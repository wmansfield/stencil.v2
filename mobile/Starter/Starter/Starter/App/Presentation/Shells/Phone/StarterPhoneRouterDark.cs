using Stencil.Maui.Commanding;
using Stencil.Maui.Presentation.Routing.Routers;
using Stencil.Maui.Presentation.Shells;
using Stencil.Maui.Views;
using Starter.App.Screens;


namespace Starter.App.Presentation.Shells.Phone
{
    public class StarterPhoneRouterDark : PhoneRouter<MenuBarViewDark>
    {
        public StarterPhoneRouterDark(ICommandProcessor commandProcessor)
            : base(commandProcessor)
        {
        }
        protected override Page GenerateBlankPage<TPage>(ShellModel shellModel)
        {
            return base.ExecuteFunction<Page>(nameof(GenerateBlankPage), delegate ()
            {
                IDataViewVisual visual = shellModel.View as IDataViewVisual;
                if (visual != null && visual.DataViewModel != null && visual.DataViewModel.Claims != null)
                {
                    if (visual.DataViewModel.Claims.Contains(WellKnownClaims.BACKGROUND_BLEED_LIGHT))
                    {
                        return new BlankPageLight();
                    }
                    else if (visual.DataViewModel.Claims.Contains(WellKnownClaims.BACKGROUND_BLEED_DARK))
                    {
                        return new BlankPageDark();
                    }
                }
                return new BlankPageDark();
            });
        }
        protected override Page GenerateMenuPage<TPage>(ShellModel shellModel)
        {
            return base.ExecuteFunction<Page>(nameof(GenerateMenuPage), delegate ()
            {
                IDataViewVisual visual = shellModel.View as IDataViewVisual;
                if (visual != null && visual.DataViewModel != null && visual.DataViewModel.Claims != null)
                {
                    if (visual.DataViewModel.Claims.Contains(WellKnownClaims.BACKGROUND_BLEED_LIGHT))
                    {
                        return new MenuPageLight();
                    }
                    else if (visual.DataViewModel.Claims.Contains(WellKnownClaims.BACKGROUND_BLEED_DARK))
                    {
                        return new MenuPageDark();
                    }
                }
                return new MenuPageDark();
            });
        }
    }
}
