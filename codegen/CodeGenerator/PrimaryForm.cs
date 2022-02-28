using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CodeGenerator
{
	public class PrimaryForm : Form
	{
		public const string PREFERENCES_FILE = "CodeGeneratorPrefs.xml";

		private Translator _translator = new Translator();

		private IContainer components = null;

		private Label label1;

		private TextBox txtDataFile;

		private Button btnBrowseData;

		private GroupBox groupBox1;

		private CheckedListBox cblTemplates;

		private Button btnBrowseTemplates;

		private Label label2;

		private TextBox txtOutputFolder;

		private Button btnBrowseOutput;

		private Button btnGenerateFiles;

		private ProgressBar pbGenStatus;

		private Label lblStatus;

		private OpenFileDialog ofdOpenFile;

		private FolderBrowserDialog fbdBrowseFolders;

		public PrimaryForm()
		{
			this.InitializeComponent();
		}

		private void btnBrowseData_Click(object sender, EventArgs e)
		{
			try
			{
				this.ofdOpenFile.Filter = "XML Data File(*.xml)|*.xml";
				this.ofdOpenFile.Title = "Select XML file to load..";
				this.ofdOpenFile.Multiselect = false;
				this.ofdOpenFile.InitialDirectory = this.txtDataFile.Text;
				if (this.ofdOpenFile.ShowDialog() == DialogResult.OK)
				{
					this._translator.DataFile = this.ofdOpenFile.FileName;
					this.txtDataFile.Text = this.ofdOpenFile.FileName;
				}
			}
			catch (Exception ex)
			{
				this.NotifyError(ex.Message, false);
			}
		}

		private void btnBrowseTemplates_Click(object sender, EventArgs e)
		{
			try
			{
				this.ofdOpenFile.Filter = "XSL Templates(*.xsl,*.xslt)|*.xsl;*.xslt";
				this.ofdOpenFile.Title = "Select templates to load..";
				this.ofdOpenFile.Multiselect = true;
				if (this.ofdOpenFile.ShowDialog() == DialogResult.OK)
				{
					this._translator.Templates.Clear();
					this.cblTemplates.Items.Clear();
					string[] fileNames = this.ofdOpenFile.FileNames;
					for (int i = 0; i < fileNames.Length; i++)
					{
						string text = fileNames[i];
						this._translator.Templates.Add(new Template(text, text, true));
					}
					foreach (Template current in this._translator.Templates)
					{
						this.cblTemplates.Items.Add(current, true);
					}
				}
			}
			catch (Exception ex)
			{
				this.NotifyError(ex.Message, false);
			}
		}

		private void btnBrowseOutput_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.fbdBrowseFolders.ShowDialog() == DialogResult.OK)
				{
					this._translator.OutputFolder = this.fbdBrowseFolders.SelectedPath;
					this.txtOutputFolder.Text = this.fbdBrowseFolders.SelectedPath;
				}
			}
			catch (Exception ex)
			{
				this.NotifyError(ex.Message, false);
			}
		}

		private void cblTemplates_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			try
			{
				if (this.cblTemplates.SelectedItem != null)
				{
					((Template)this.cblTemplates.SelectedItem).IsSelected = (e.NewValue == CheckState.Checked);
				}
			}
			catch (Exception ex)
			{
				this.NotifyError(ex.Message, false);
			}
		}

		private void btnGenerateFiles_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.txtDataFile.Text))
				{
					this.NotifyError("You must provide a data file.", true);
				}
				else
				{
					bool flag = false;
					foreach (Template current in this._translator.Templates)
					{
						if (current.IsSelected)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.NotifyError("You must have at least one template loaded and selected.", true);
					}
					else
					{
						this._translator.DataFile = this.txtDataFile.Text;
						this._translator.OutputFolder = this.txtOutputFolder.Text;
						this.btnGenerateFiles.Enabled = false;
						this._translator.GenFiles();
					}
				}
			}
			catch (Exception ex)
			{
				this.lblStatus.Text = "Error Occurred";
				this.pbGenStatus.Value = 0;
				this.NotifyError(ex.Message, false);
			}
			finally
			{
				this.btnGenerateFiles.Enabled = true;
				this.btnGenerateFiles.Focus();
			}
		}

		private void _translator_Progress(object sender, TranslatorEventArgs e)
		{
			try
			{
				base.Invoke(new ThreadStart(delegate
				{
					this.pbGenStatus.Value = Convert.ToInt32(e.Progress);
				}));
			}
			catch (Exception ex)
			{
				this.NotifyError(ex.Message, false);
			}
		}

		private void _translator_Notice(object sender, TranslatorEventArgs e)
		{
			try
			{
				base.Invoke(new ThreadStart(delegate
				{
					this.lblStatus.Text = e.Message;
					this.Refresh();
				}));
			}
			catch (Exception ex)
			{
				this.NotifyError(ex.Message, false);
			}
		}

		private void _translator_Error(object sender, TranslatorEventArgs e)
		{
			try
			{
				base.Invoke(new ThreadStart(delegate
				{
					this.NotifyError(e.Message, true);
				}));
			}
			catch (Exception ex)
			{
				this.NotifyError(ex.Message, false);
			}
		}

		private void NotifyError(string message, bool IsHandled = false)
		{
			if (IsHandled)
			{
				MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				MessageBox.Show(message + "\rNOTE:  Program may not function properly.", "Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void PrimaryForm_Shown(object sender, EventArgs e)
		{
			try
			{
				this._translator.Error += new EventHandler<TranslatorEventArgs>(this._translator_Error);
				this._translator.Notice += new EventHandler<TranslatorEventArgs>(this._translator_Notice);
				this._translator.Progress += new EventHandler<TranslatorEventArgs>(this._translator_Progress);
				try
				{
					Options options = Utility.DeserializeFromXml<Options>(new FileInfo("CodeGeneratorPrefs.xml"));
					if (options != null)
					{
						this._translator.DataFile = options.DataFile;
						this.txtDataFile.Text = options.DataFile;
						this._translator.OutputFolder = options.OutputFolder;
						this.txtOutputFolder.Text = options.OutputFolder;
						foreach (string current in options.SelectedFiles)
						{
							this._translator.Templates.Add(new Template(current, current, true));
						}
						foreach (string current in options.UnSelectedFiles)
						{
							this._translator.Templates.Add(new Template(current, current, false));
						}
						foreach (Template current2 in this._translator.Templates)
						{
							this.cblTemplates.Items.Add(current2, current2.IsSelected);
						}
					}
				}
				catch
				{
				}
			}
			catch (Exception ex)
			{
				this.NotifyError(ex.Message, false);
			}
		}

		private void PrimaryForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				Options options = new Options();
				options.OutputFolder = this.txtOutputFolder.Text;
				options.DataFile = this.txtDataFile.Text;
				foreach (Template current in this._translator.Templates)
				{
					if (current.IsSelected)
					{
						options.SelectedFiles.Add(current.Location);
					}
					else
					{
						options.UnSelectedFiles.Add(current.Location);
					}
				}
				Utility.SerializeToXml<Options>(options, new FileInfo("CodeGeneratorPrefs.xml"));
			}
			catch
			{
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PrimaryForm));
			this.label1 = new Label();
			this.txtDataFile = new TextBox();
			this.btnBrowseData = new Button();
			this.groupBox1 = new GroupBox();
			this.cblTemplates = new CheckedListBox();
			this.btnBrowseTemplates = new Button();
			this.label2 = new Label();
			this.txtOutputFolder = new TextBox();
			this.btnBrowseOutput = new Button();
			this.btnGenerateFiles = new Button();
			this.pbGenStatus = new ProgressBar();
			this.lblStatus = new Label();
			this.ofdOpenFile = new OpenFileDialog();
			this.fbdBrowseFolders = new FolderBrowserDialog();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(34, 28);
			this.label1.Name = "label1";
			this.label1.Size = new Size(53, 14);
			this.label1.TabIndex = 0;
			this.label1.Text = "Data File";
			this.txtDataFile.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtDataFile.Location = new Point(93, 25);
			this.txtDataFile.Name = "txtDataFile";
			this.txtDataFile.Size = new Size(506, 22);
			this.txtDataFile.TabIndex = 0;
			this.btnBrowseData.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.btnBrowseData.Location = new Point(605, 22);
			this.btnBrowseData.Name = "btnBrowseData";
			this.btnBrowseData.Size = new Size(35, 25);
			this.btnBrowseData.TabIndex = 1;
			this.btnBrowseData.Text = "...";
			this.btnBrowseData.UseVisualStyleBackColor = true;
			this.btnBrowseData.Click += new EventHandler(this.btnBrowseData_Click);
			this.groupBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.groupBox1.Controls.Add(this.cblTemplates);
			this.groupBox1.Controls.Add(this.btnBrowseTemplates);
			this.groupBox1.Location = new Point(15, 65);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(632, 194);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Translator Templates";
			this.cblTemplates.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.cblTemplates.BorderStyle = BorderStyle.FixedSingle;
			this.cblTemplates.FormattingEnabled = true;
			this.cblTemplates.Location = new Point(6, 48);
			this.cblTemplates.Name = "cblTemplates";
			this.cblTemplates.Size = new Size(620, 138);
			this.cblTemplates.TabIndex = 1;
			this.cblTemplates.ItemCheck += new ItemCheckEventHandler(this.cblTemplates_ItemCheck);
			this.btnBrowseTemplates.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.btnBrowseTemplates.Location = new Point(495, 16);
			this.btnBrowseTemplates.Name = "btnBrowseTemplates";
			this.btnBrowseTemplates.Size = new Size(130, 25);
			this.btnBrowseTemplates.TabIndex = 0;
			this.btnBrowseTemplates.Text = "Load Templates..";
			this.btnBrowseTemplates.UseVisualStyleBackColor = true;
			this.btnBrowseTemplates.Click += new EventHandler(this.btnBrowseTemplates_Click);
			this.label2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.label2.AutoSize = true;
			this.label2.Location = new Point(12, 282);
			this.label2.Name = "label2";
			this.label2.Size = new Size(82, 14);
			this.label2.TabIndex = 4;
			this.label2.Text = "Output Folder";
			this.txtOutputFolder.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.txtOutputFolder.Location = new Point(100, 279);
			this.txtOutputFolder.Name = "txtOutputFolder";
			this.txtOutputFolder.Size = new Size(506, 22);
			this.txtOutputFolder.TabIndex = 2;
			this.btnBrowseOutput.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.btnBrowseOutput.Location = new Point(612, 277);
			this.btnBrowseOutput.Name = "btnBrowseOutput";
			this.btnBrowseOutput.Size = new Size(35, 25);
			this.btnBrowseOutput.TabIndex = 3;
			this.btnBrowseOutput.Text = "...";
			this.btnBrowseOutput.UseVisualStyleBackColor = true;
			this.btnBrowseOutput.Click += new EventHandler(this.btnBrowseOutput_Click);
			this.btnGenerateFiles.Anchor = AnchorStyles.Bottom;
			this.btnGenerateFiles.Location = new Point(261, 349);
			this.btnGenerateFiles.Name = "btnGenerateFiles";
			this.btnGenerateFiles.Size = new Size(133, 43);
			this.btnGenerateFiles.TabIndex = 4;
			this.btnGenerateFiles.Text = "Generate Files";
			this.btnGenerateFiles.UseVisualStyleBackColor = true;
			this.btnGenerateFiles.Click += new EventHandler(this.btnGenerateFiles_Click);
			this.pbGenStatus.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.pbGenStatus.Location = new Point(12, 318);
			this.pbGenStatus.Name = "pbGenStatus";
			this.pbGenStatus.Size = new Size(635, 25);
			this.pbGenStatus.TabIndex = 8;
			this.lblStatus.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.lblStatus.Font = new Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lblStatus.Location = new Point(433, 363);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new Size(207, 25);
			this.lblStatus.TabIndex = 9;
			this.lblStatus.TextAlign = ContentAlignment.MiddleRight;
			base.AutoScaleDimensions = new SizeF(7f, 14f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(659, 404);
			base.Controls.Add(this.lblStatus);
			base.Controls.Add(this.pbGenStatus);
			base.Controls.Add(this.btnGenerateFiles);
			base.Controls.Add(this.btnBrowseOutput);
			base.Controls.Add(this.txtOutputFolder);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.btnBrowseData);
			base.Controls.Add(this.txtDataFile);
			base.Controls.Add(this.label1);
			this.Font = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.Name = "PrimaryForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Code Generation Utility";
			base.FormClosing += new FormClosingEventHandler(this.PrimaryForm_FormClosing);
			base.Shown += new EventHandler(this.PrimaryForm_Shown);
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
