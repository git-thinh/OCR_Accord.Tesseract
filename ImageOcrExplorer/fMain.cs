using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageOcrExplorer
{
    public class fMain : Form
    {
        #region [ FORM BASE ]

        public fMain()
        {
            f_base_Init();
        }

        MenuStrip ui_menu;
        ToolStripMenuItem ui_menuFile;
        ToolStripMenuItem ui_menuFilter;
        ToolStripMenuItem ui_menuScript;
        Panel ui_panelExplorer;
        Panel ui_panelImages;

        Image ui_iconFolder = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.FolderOpen) { ForeColor = Color.OrangeRed, Size = 24, BackColor = Color.Transparent });
        Image ui_iconSave = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.Save) { ForeColor = Color.Black, Size = 20, BackColor = Color.Transparent });
        Image ui_iconAddNew = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.PlusSquare) { ForeColor = Color.Black, Size = 20, BackColor = Color.Transparent });
        Image ui_iconRemove = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.TrashO) { ForeColor = Color.Black, Size = 20, BackColor = Color.Transparent });
        Image ui_iconScriptAdd = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.Codepen) { ForeColor = Color.Black, Size = 24, BackColor = Color.Transparent });
        Image ui_iconImage = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.Image) { ForeColor = Color.OrangeRed, Size = 24, BackColor = Color.Transparent });

        ComboBox ui_filterComboBox;
        ComboBox ui_scriptComboBox;

        CheckedListBox ui_scriptListFilters;
        CheckedListBox ui_fileList;

        FlowLayoutPanel ui_filterConfigForm;

        void f_base_Init()
        {
            this.Text = "Test Image Ocr";

            ui_menu = new MenuStrip();

            ui_menuFile = new ToolStripMenuItem("File");
            ui_menuFilter = new ToolStripMenuItem("Filter");
            ui_menuScript = new ToolStripMenuItem("Script");
            ui_menu.Items.AddRange(new ToolStripItem[] { ui_menuFile, ui_menuFilter, ui_menuScript });

            ui_menuFile.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("Open Folder", null, new EventHandler((se,ev) => f_execute_features(TYPE_FEATURE.OPEN_FOLDER))),
                new ToolStripMenuItem("Open Files", null, new EventHandler((se,ev) => f_execute_features(TYPE_FEATURE.OPEN_FILES))),
                new ToolStripMenuItem("Recent Opened", null, new EventHandler((se,ev) => f_execute_features(TYPE_FEATURE.RECENT_OPEN))),
                new ToolStripMenuItem("Exit", null, new EventHandler((se,ev) => f_execute_features(TYPE_FEATURE.EXIT))),
            });

            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
             
            ui_panelExplorer = new Panel() { Dock = DockStyle.Left, Width = 250, BackColor = Color.Gray };
            this.Controls.Add(ui_panelExplorer);

            var ui_hrBox = new Panel() { Dock = DockStyle.Left, Width = 3, BackColor = Color.DodgerBlue, Padding = new Padding(0) };
            this.Controls.Add(ui_hrBox);
            ui_hrBox.BringToFront();

            ui_panelImages = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White };
            this.Controls.Add(ui_panelImages);

            //this.Shown += (se, ev) => {
            //    ui_panelExplorer.SendToBack();
            //    ui_panelImages.BringToFront(); 
            //};
            this.Controls.Add(ui_menu);

            //----------------------------------------------------------------------------------------------------------------------------
            // FILTER

            var ui_filterRemove = new PictureBox() {  Image = ui_iconRemove, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage };
            var ui_filterApplyToImage = new PictureBox() { Image = ui_iconImage, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage }; 
            
            ui_filterComboBox = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
            var ui_filterBox = new Panel() { Dock = DockStyle.Top, Height = 25, BackColor = Color.DodgerBlue, Padding = new Padding(0, 3, 0, 0) };
            ui_filterBox.Controls.AddRange(new Control[] {
                new Label() { Text = "Filters", Dock = DockStyle.Left, Width = 35, TextAlign = ContentAlignment.MiddleCenter },
                ui_filterComboBox, 
                ui_filterRemove,
                new Label() { Text = "", Dock = DockStyle.Right, Width = 3 },
                ui_filterApplyToImage });
            ui_panelExplorer.Controls.Add(ui_filterBox);
            ui_filterRemove.Click += (se, ev) => { f_execute_features(TYPE_FEATURE.FILTER_REMOVE_FROM_SCRIPT, ui_filterComboBox.SelectedText); };

            ui_filterConfigForm = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 150, BackColor = Color.DodgerBlue }; 
            ui_panelExplorer.Controls.Add(ui_filterConfigForm);
            ui_filterConfigForm.BringToFront();

            //----------------------------------------------------------------------------------------------------------------------------
            // HR SPACE
            var ui_hrBox1 = new Panel() { Dock = DockStyle.Top, Height = 11, BackColor = Color.DodgerBlue, Padding = new Padding(0, 5, 0, 5) };
            ui_hrBox1.Controls.Add(new Label() { AutoSize = false, Text = "", Height = 1, Dock = DockStyle.Top, BackColor = Color.Black });
            ui_panelExplorer.Controls.Add(ui_hrBox1);
            ui_hrBox1.BringToFront();
            
            //----------------------------------------------------------------------------------------------------------------------------
            // SCRIPT

            var ui_scriptRemove = new PictureBox() { Image = ui_iconRemove, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage };
            var ui_scriptApplyToImage = new PictureBox() { Image = ui_iconImage, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage };

            ui_scriptComboBox = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
            var ui_scriptBox = new Panel() { Dock = DockStyle.Top, Height = 22, BackColor = Color.DodgerBlue };
            ui_scriptBox.Controls.AddRange(new Control[] {
                new Label() { Text = "Script", Dock = DockStyle.Left, Width = 35, TextAlign = ContentAlignment.MiddleCenter },
                ui_scriptComboBox,
                ui_scriptRemove, 
                new Label() { Text = "", Dock = DockStyle.Right, Width = 3 },
                ui_scriptApplyToImage });
            ui_panelExplorer.Controls.Add(ui_scriptBox);
            ui_scriptRemove.Click += (se, ev) => { f_execute_features(TYPE_FEATURE.FILTER_REMOVE_FROM_SCRIPT, ui_filterComboBox.SelectedText); };
            ui_scriptBox.BringToFront();
            
            ui_scriptListFilters = new CheckedListBox() { Dock = DockStyle.Top, Height = 150, BackColor = Color.DodgerBlue, BorderStyle = BorderStyle.None };
            ui_panelExplorer.Controls.Add(ui_scriptListFilters);
            ui_scriptListFilters.BringToFront();

            //----------------------------------------------------------------------------------------------------------------------------
            // HR SPACE
            var ui_hrBox2 = new Panel() { Dock = DockStyle.Top, Height = 11, BackColor = Color.DodgerBlue, Padding = new Padding(0, 5, 0, 5) };
            ui_hrBox2.Controls.Add(new Label() { AutoSize = false, Text = "", Height = 1, Dock = DockStyle.Top, BackColor = Color.Black });
            ui_panelExplorer.Controls.Add(ui_hrBox2);
            ui_hrBox2.BringToFront();

            //----------------------------------------------------------------------------------------------------------------------------
            // LIST FILES


            var ui_fileOpen = new PictureBox() { Image = ui_iconFolder, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage }; 

            var ui_fileSearchTextbox = new TextBox() { Dock = DockStyle.Fill, BackColor = SystemColors.Control };
            var ui_fileBox = new Panel() { Dock = DockStyle.Top, Height = 25, BackColor = Color.DodgerBlue, Padding = new Padding(0, 0, 0, 3) };
            ui_fileBox.Controls.AddRange(new Control[] {
                new Label() { Text = "Files", Dock = DockStyle.Left, Width = 35, TextAlign = ContentAlignment.MiddleCenter },
                ui_fileSearchTextbox,
                ui_fileOpen});
            ui_panelExplorer.Controls.Add(ui_fileBox);
            ui_fileOpen.Click += (se, ev) => { f_execute_features(TYPE_FEATURE.FILTER_REMOVE_FROM_SCRIPT, ui_filterComboBox.SelectedText); };
            ui_fileBox.BringToFront();

            ui_fileList = new CheckedListBox() { Dock = DockStyle.Fill, Height = 150, BackColor = SystemColors.Control, BorderStyle = BorderStyle.None };
            ui_panelExplorer.Controls.Add(ui_fileList);
            ui_fileList.BringToFront(); 
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            var confirmResult = MessageBox.Show("Are you sure to exit application?", "Confirm Exit", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Dispose(true);
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        #endregion

        object f_execute_features(TYPE_FEATURE type, object para = null)
        {
            switch (type)
            {
                case TYPE_FEATURE.EXIT:
                    this.Close();
                    break;
                case TYPE_FEATURE.OPEN_FOLDER:
                    break;
                case TYPE_FEATURE.OPEN_FILES:
                    break;
                case TYPE_FEATURE.RECENT_OPEN:
                    break;

                case TYPE_FEATURE.FILTER_INSERT_INTO_SCRIPT:
                    break;
            }

            return null;
        }

    }
}
