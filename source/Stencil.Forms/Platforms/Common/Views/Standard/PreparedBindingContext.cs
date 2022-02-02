using Newtonsoft.Json;
using Stencil.Forms.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Stencil.Forms.Views.Standard
{
    public abstract class PreparedBindingContext : TrackedClass, IDataViewItemReference
    {
        public PreparedBindingContext(string trackPrefix)
            : base(trackPrefix)
        {

        }

        public List<InteractionStateGroup> InteractionStateGroups { get; set; }


        [JsonIgnore]
        public virtual Dictionary<string, Dictionary<string, List<InteractionStateGroup>>> InteractionLookup { get; set; }

        [JsonIgnore]
        public IDataViewItem DataViewItem { get; set; }

        [JsonIgnore]
        public ICommandScope CommandScope { get; set; }

        protected bool HasPrepared { get; set; }

        public string[] GetInteractionGroups()
        {
            return base.ExecuteFunction(nameof(GetInteractionGroups), delegate ()
            {
                this.PrepareInteractions(false);

                if(this.InteractionLookup != null)
                {
                    return this.InteractionLookup.Keys.ToArray();
                }
                return null;
            });
        }
        public virtual void PrepareInteractions(bool force = false)
        {
            base.ExecuteMethod(nameof(PrepareInteractions), delegate ()
            {
                if(!force && this.HasPrepared)
                {
                    return;
                }
                this.HasPrepared = true; // proactive, ignore error

                Dictionary<string, Dictionary<string, List<InteractionStateGroup>>> lookup = null;
                if (this.InteractionStateGroups != null)
                {
                    lookup = new Dictionary<string, Dictionary<string, List<InteractionStateGroup>>>(StringComparer.OrdinalIgnoreCase);
                    foreach (InteractionStateGroup item in this.InteractionStateGroups)
                    {
                        if(item.state_key == null) { item.state_key = string.Empty; }

                        if(!lookup.ContainsKey(item.interaction_group))
                        {
                            lookup[item.interaction_group] = new Dictionary<string, List<InteractionStateGroup>>(StringComparer.OrdinalIgnoreCase);
                        }
                        Dictionary<string, List<InteractionStateGroup>> groupLookup = lookup[item.interaction_group];
                        if (!groupLookup.ContainsKey(item.state_key))
                        {
                            groupLookup[item.state_key] = new List<InteractionStateGroup>();
                        }
                        groupLookup[item.state_key].Add(item);
                    }

                }
                this.InteractionLookup = lookup;
            });
        }

        public virtual void OnInteractionStateChanged(string group, string key, string state)
        {
            base.ExecuteMethod(nameof(OnInteractionStateChanged), delegate ()
            {
                if (this.InteractionLookup == null)
                {
                    return;
                }
                if(group == null)
                {
                    group = string.Empty;
                }
                if (key == null)
                {
                    key = string.Empty;
                }

                

                Dictionary<string, List<InteractionStateGroup>> groupResponders = null;
                if(this.InteractionLookup.TryGetValue(group, out groupResponders) && groupResponders != null)
                {
                    List<InteractionStateGroup> responders = null;
                    if (groupResponders.TryGetValue(key, out responders) && responders != null)
                    {
                        foreach (InteractionStateGroup stateGroup in responders)
                        {
                            if (stateGroup.interaction_group == group && stateGroup.state_key == key)
                            {
                                bool applyChange = false;
                                string value = null;
                                foreach (InteractionStateMap map in stateGroup.state_maps)
                                {
                                    bool matched = false;

                                    switch (map.state_operator)
                                    {
                                        case InteractionStateOperator.equals:
                                            matched = map.state == state;
                                            break;
                                        case InteractionStateOperator.not_equals:
                                            matched = map.state != state;
                                            break;
                                        case InteractionStateOperator.any:
                                            matched = true;
                                            break;
                                        default:
                                            break;
                                    }
                                    if (matched)
                                    {
                                        applyChange = true;
                                        if (string.IsNullOrEmpty(map.value_format))
                                        {
                                            value = state;
                                        }
                                        else
                                        {
                                            value = string.Format(map.value_format, state);
                                        }
                                    }
                                }

                                if (applyChange)
                                {
                                    this.ApplyStateValue(stateGroup.interaction_group, stateGroup.state_key, state, stateGroup.value_key, value);
                                }
                            }
                        }
                    }
                }
            });
        }
        

        protected virtual void ApplyStateValue(string group, string state_key, string state, string value_key, string value)
        {
            // designed for override
        }


    }
}
