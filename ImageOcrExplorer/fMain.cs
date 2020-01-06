using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;

namespace ImageOcrExplorer
{
    public class fMain : Form
    {
        #region [ MEMBER ]

        int IMG_WIDTH = 0, IMG_HEIGHT = 0;
        bool FILTER_EXECUTE_AUTO_CHANGED = true;

        string[] M_FILES = new string[] { };
        string M_FILTER_SELECTED = "";
        string M_SCRIPT_SELECTED = "";

        #endregion

        #region [ VARIABLE ]

        MenuStrip ui_menu;
        ToolStripMenuItem ui_menuFile;
        ToolStripMenuItem ui_menuFilter;
        ToolStripMenuItem ui_menuScript;
        Panel ui_panelExplorer;
        FlowLayoutPanel ui_panelImages;

        Image ui_iconFolder = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.FolderOpen) { ForeColor = Color.Black, Size = 24, BackColor = Color.Transparent });
        Image ui_iconSave = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.Save) { ForeColor = Color.Black, Size = 20, BackColor = Color.Transparent });
        Image ui_iconAddNew = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.PlusSquare) { ForeColor = Color.Black, Size = 20, BackColor = Color.Transparent });
        Image ui_iconRemove = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.TrashO) { ForeColor = Color.Black, Size = 20, BackColor = Color.Transparent });
        Image ui_iconScriptAdd = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.Codepen) { ForeColor = Color.Black, Size = 24, BackColor = Color.Transparent });
        Image ui_iconImage = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.Image) { ForeColor = Color.OrangeRed, Size = 24, BackColor = Color.Transparent });

        ToolTipComboBox ui_filterComboBox;
        ComboBox ui_scriptComboBox;

        CheckedListBox ui_scriptListFilters;
        FlowLayoutPanel ui_fileList;

        FlowLayoutPanel ui_filterConfigForm;

        PictureBox ui_imageOrigin;
        
        ToolTip ui_tooltip;



        #endregion

        #region [ FORM BASE ]

        public fMain()
        {
            f_base_Init();
        }

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
            //this.MaximizeBox = false;

            ui_panelExplorer = new Panel() { Dock = DockStyle.Left, Width = 250, BackColor = Color.Gray };
            this.Controls.Add(ui_panelExplorer);

            var ui_hrBox = new Panel() { Dock = DockStyle.Left, Width = 3, BackColor = Color.DodgerBlue, Padding = new Padding(0) };
            this.Controls.Add(ui_hrBox);
            ui_hrBox.BringToFront();

            ui_panelImages = new FlowLayoutPanel() { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true };
            this.Controls.Add(ui_panelImages);
            ui_panelImages.BringToFront();

            this.Controls.Add(ui_menu);
            //----------------------------------------------------------------------------------------------------------------------------
            // FILTER

            ui_tooltip = new ToolTip();

            // Set up the delays for the ToolTip.
            ui_tooltip.AutoPopDelay = 5000;
            ui_tooltip.InitialDelay = 1000;
            ui_tooltip.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            ui_tooltip.ShowAlways = true;

            //----------------------------------------------------------------------------------------------------------------------------
            // FILTER

            var ui_filterAddToScriptList = new PictureBox() { Image = ui_iconAddNew, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage };
            var ui_filterRemove = new PictureBox() { Image = ui_iconRemove, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage };
            var ui_filterApplyToImage = new PictureBox() { Image = ui_iconImage, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage };

            ui_filterComboBox = new ToolTipComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
            ui_filterComboBox.SelectedIndexChanged += (se, ev) => { M_FILTER_SELECTED = ui_filterComboBox.SelectedItem as string; };

            var ui_filterBox = new Panel() { Dock = DockStyle.Top, Height = 25, BackColor = Color.DodgerBlue, Padding = new Padding(0, 3, 0, 0) };
            ui_filterBox.Controls.AddRange(new Control[] {
                //new Label() { Text = "Filters", Dock = DockStyle.Left, Width = 35, TextAlign = ContentAlignment.MiddleCenter },
                ui_filterComboBox,
                ui_filterAddToScriptList,
                new Label() { Text = "", Dock = DockStyle.Right, Width = 3 },
                ui_filterRemove,
                new Label() { Text = "", Dock = DockStyle.Right, Width = 3 },
                ui_filterApplyToImage });
            ui_panelExplorer.Controls.Add(ui_filterBox);
            ui_filterRemove.Click += (se, ev) => { f_execute_features(TYPE_FEATURE.FILTER_REMOVE_FROM_SCRIPT, M_FILTER_SELECTED); };
            ui_filterApplyToImage.Click += (se, ev) => {
                f_execute_features(TYPE_FEATURE.FILTER_SELECTED_ITEM_EXECUTE, M_FILTER_SELECTED);
            };

            ui_filterConfigForm = new FlowLayoutPanel() { Dock = DockStyle.Top, Height = 150, BackColor = Color.DodgerBlue };
            ui_panelExplorer.Controls.Add(ui_filterConfigForm);
            ui_filterConfigForm.BringToFront();
            ui_filterComboBox.BringToFront();

            ui_tooltip.SetToolTip(ui_filterAddToScriptList, "Insert the filter into Script List");
            ui_tooltip.SetToolTip(ui_filterRemove, "Remove the filter into Script List");
            ui_tooltip.SetToolTip(ui_filterApplyToImage, "Apply the filter for selected images");

            //----------------------------------------------------------------------------------------------------------------------------
            // HR SPACE
            var ui_hrBox1 = new Panel() { Dock = DockStyle.Top, Height = 11, BackColor = Color.DodgerBlue, Padding = new Padding(0, 5, 0, 5) };
            ui_hrBox1.Controls.Add(new Label() { AutoSize = false, Text = "", Height = 1, Dock = DockStyle.Top, BackColor = Color.Black });
            ui_panelExplorer.Controls.Add(ui_hrBox1);
            ui_hrBox1.BringToFront();

            //----------------------------------------------------------------------------------------------------------------------------
            // SCRIPT

            var ui_scriptSave = new PictureBox() { Image = ui_iconSave, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage };
            var ui_scriptRemove = new PictureBox() { Image = ui_iconRemove, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage };
            var ui_scriptApplyToImage = new PictureBox() { Image = ui_iconImage, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage };

            ui_scriptComboBox = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
            ui_scriptComboBox.SelectedIndexChanged += (se, ev) => { M_SCRIPT_SELECTED = ui_scriptComboBox.SelectedItem as string; };
            var ui_scriptBox = new Panel() { Dock = DockStyle.Top, Height = 22, BackColor = Color.DodgerBlue };
            ui_scriptBox.Controls.AddRange(new Control[] {
                //new Label() { Text = "Script", Dock = DockStyle.Left, Width = 35, TextAlign = ContentAlignment.MiddleCenter },
                ui_scriptComboBox,
                ui_scriptSave,
                new Label() { Text = "", Dock = DockStyle.Right, Width = 3 },
                ui_scriptRemove,
                new Label() { Text = "", Dock = DockStyle.Right, Width = 3 },
                ui_scriptApplyToImage });
            ui_panelExplorer.Controls.Add(ui_scriptBox);
            ui_scriptRemove.Click += (se, ev) => { f_execute_features(TYPE_FEATURE.FILTER_REMOVE_FROM_SCRIPT, ui_scriptComboBox.SelectedItem); };
            ui_scriptBox.BringToFront();
            ui_scriptComboBox.BringToFront();

            ui_scriptListFilters = new CheckedListBox() { Dock = DockStyle.Top, Height = 150, BackColor = Color.DodgerBlue, BorderStyle = BorderStyle.None };
            ui_panelExplorer.Controls.Add(ui_scriptListFilters);
            ui_scriptListFilters.BringToFront();

            ui_tooltip.SetToolTip(ui_scriptSave, "Save Script List");
            ui_tooltip.SetToolTip(ui_scriptRemove, "Remove Script List");
            ui_tooltip.SetToolTip(ui_scriptApplyToImage, "Apply the selected script for selected images");

            //----------------------------------------------------------------------------------------------------------------------------
            // HR SPACE
            var ui_hrBox2 = new Panel() { Dock = DockStyle.Top, Height = 11, BackColor = Color.DodgerBlue, Padding = new Padding(0, 5, 0, 5) };
            ui_hrBox2.Controls.Add(new Label() { AutoSize = false, Text = "", Height = 1, Dock = DockStyle.Top, BackColor = Color.Black });
            ui_panelExplorer.Controls.Add(ui_hrBox2);
            ui_hrBox2.BringToFront();

            //----------------------------------------------------------------------------------------------------------------------------
            // LIST FILES

            var ui_fileOpen = new PictureBox() { Image = ui_iconFolder, Dock = DockStyle.Right, Width = 24, SizeMode = PictureBoxSizeMode.CenterImage };
            ui_fileOpen.Click += (se, ev) => { f_execute_features(TYPE_FEATURE.OPEN_FILES); };
            var ui_fileSearchTextbox = new TextBox() { Dock = DockStyle.Fill, BackColor = SystemColors.Control };
            var ui_fileBox = new Panel() { Dock = DockStyle.Top, Height = 25, BackColor = Color.DodgerBlue, Padding = new Padding(0, 0, 0, 3) };
            ui_fileBox.Controls.AddRange(new Control[] {
                new Label() { Text = "Files", Dock = DockStyle.Left, Width = 35, TextAlign = ContentAlignment.MiddleCenter },
                ui_fileSearchTextbox,
                ui_fileOpen});
            ui_panelExplorer.Controls.Add(ui_fileBox);
            ui_fileBox.BringToFront();

            ui_fileList = new FlowLayoutPanel()
            {
                AutoScroll = true,
                RightToLeft = RightToLeft.Yes,
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                Height = 150,
                BackColor = Color.DodgerBlue,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            ui_panelExplorer.Controls.Add(ui_fileList);
            ui_fileList.BringToFront();

            //----------------------------------------------------------------------------------------------------------------------------
            // LIST FILES

            ui_imageOrigin = new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.DodgerBlue
            };
            ui_panelImages.Controls.Add(ui_imageOrigin);

            // Set up the ToolTip text for the Button and Checkbox.
            ui_tooltip.SetToolTip(this.ui_filterComboBox, "Chose Filter");
            ui_tooltip.SetToolTip(this.ui_scriptComboBox, "Chose Script");

            //----------------------------------------------------------------------------------------------------------------------------

            this.Shown += (se, ev) =>
            {
                IMG_WIDTH = (int)((Screen.PrimaryScreen.WorkingArea.Width - 250) / 3);
                IMG_HEIGHT = IMG_WIDTH * 4 / 6;

                this.Top = 0;
                this.Left = 0;
                this.Width = Screen.PrimaryScreen.WorkingArea.Width;
                this.Height = Screen.PrimaryScreen.WorkingArea.Height;

                ui_imageOrigin.Width = IMG_WIDTH;
                ui_imageOrigin.Height = IMG_HEIGHT;

                f_base_Binding();
            };

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
        
        void f_base_Binding()
        {
            List<string> ls = new List<string>();
            ls.AddRange(___IMAGE_FILTERS.getScriptNames());
            ls.AddRange(___IMAGE_DIRECTION.getScriptNames());
            ls.AddRange(___OCR.getScriptNames());
            ls = ls.OrderBy(x => x).ToList();

            ui_filterComboBox.Items.AddRange(ls.ToArray());
            f_base_menuFilterBuilding();
            f_base_menuScriptBuilding();
        }

        void f_base_menuFilterBuilding()
        {
            ui_menuFilter.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("Filter execute auto changed", null, new EventHandler((se,ev) => f_execute_features(TYPE_FEATURE.FILTER_EXECUTE_AUTO_CHANGED, se))){ Checked = FILTER_EXECUTE_AUTO_CHANGED },
                new ToolStripMenuItem("Run All Filters", null, new EventHandler((se,ev) => f_execute_features(TYPE_FEATURE.FILTER_RUN_ALL))),
                new ToolStripMenuItem("Clear All Filters", null, new EventHandler((se,ev) => f_execute_features(TYPE_FEATURE.FILTER_CLEAR_ALL))),
            });
        }

        void f_base_menuScriptBuilding()
        {
            ui_menuFilter.DropDownItems.AddRange(new ToolStripItem[] {
                //new ToolStripMenuItem("Open Folder", null, new EventHandler((se,ev) => f_execute_features(TYPE_FEATURE.OPEN_FOLDER))),
            });
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
                    #region

                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
                    dlg.Multiselect = true;
                    dlg.Title = "Select images";

                    DialogResult dr = dlg.ShowDialog();
                    if (dr == DialogResult.OK)
                        f_execute_features(TYPE_FEATURE.IMAGE_ORIGIN_SELECTED_BINDING, dlg.FileNames);

                    #endregion
                    break;
                case TYPE_FEATURE.RECENT_OPEN:
                    break;

                case TYPE_FEATURE.IMAGE_ORIGIN_SELECTED_ITEM:
                    #region

                    if (para != null)
                    {
                        string file = para as string;
                        if (File.Exists(file))
                        {
                            ui_imageOrigin.ImageLocation = file;
                        }
                    }

                    #endregion
                    break;
                case TYPE_FEATURE.IMAGE_ORIGIN_SELECTED_BINDING:
                    #region

                    if (para != null)
                    {
                        M_FILES = para as string[];

                        int w = ui_fileList.Width - 30;
                        foreach (String file in M_FILES)
                        {
                            try
                            {
                                ui_fileList.Controls.Add(new Label()
                                {
                                    Width = w,
                                    Text = Path.GetFileName(file),
                                    Height = 16,
                                    RightToLeft = RightToLeft.No
                                });

                                PictureBox p = new PictureBox();
                                Image img = Image.FromFile(file);
                                int h = (int)((ui_fileList.Width * img.Height) / img.Width);

                                p.Width = w;
                                p.Height = h;

                                p.Image = img;
                                p.SizeMode = PictureBoxSizeMode.StretchImage;
                                ui_fileList.Controls.Add(p);

                                ui_fileList.Controls.Add(new Label() { Width = w, Text = "", Height = 7 });

                                p.Click += (se, ev) => f_execute_features(TYPE_FEATURE.IMAGE_ORIGIN_SELECTED_ITEM, file);
                            }
                            catch (SecurityException ex)
                            {
                                // The user lacks appropriate permissions to read files, discover paths, etc.
                                MessageBox.Show("Security error. Please contact your administrator for details.\n\n" +
                                    "Error message: " + ex.Message + "\n\n" +
                                    "Details (send to Support):\n\n" + ex.StackTrace
                                );
                            }
                            catch (Exception ex)
                            {
                                // Could not load the image - probably related to Windows file system permissions.
                                MessageBox.Show("Cannot display the image: " + file.Substring(file.LastIndexOf('\\'))
                                    + ". You may not have permission to read the file, or " +
                                    "it may be corrupt.\n\nReported error: " + ex.Message);
                            }
                        }
                    }

                    #endregion
                    break;

                case TYPE_FEATURE.FILTER_CLEAR_ALL:
                    #region

                    ui_panelImages.Controls.Clear();
                    ui_panelImages.Controls.Add(ui_imageOrigin);

                    #endregion
                    break;
                case TYPE_FEATURE.FILTER_RUN_ALL:
                    #region
                    if (ui_imageOrigin.Image != null)
                    {
                        //Bitmap img = (ui_imageOrigin.Image as Bitmap).CloneBitmap();
                        Bitmap img = ui_imageOrigin.Image as Bitmap;
                        int w = ui_panelImages.Width / 3 - 30;
                        int h = w * 4 / 6;


                        foreach (string filter in ___IMAGE_FILTERS.getScriptNames())
                        {
                            object config = null;
                            Bitmap image = ___IMAGE_FILTERS.Execute(filter, config, img);

                            //ui_panelImages.Controls.Add(new Label()
                            //{
                            //    Width = IMG_WIDTH,
                            //    Text = filter,
                            //    Height = 16,
                            //    RightToLeft = RightToLeft.No
                            //});

                            PictureBox p = new PictureBox() { Width = w, Height = h };
                            p.Image = image;
                            p.SizeMode = PictureBoxSizeMode.StretchImage;
                            ui_panelImages.Controls.Add(p);

                            //ui_panelImages.Controls.Add(new Label() { Width = IMG_WIDTH, Text = "", Height = 7 });

                            ////p.Click += (se, ev) => f_execute_features(TYPE_FEATURE.IMAGE_ORIGIN_SELECTED_ITEM, file);
                            //Thread.Sleep(100);
                        }
                    }

                    #endregion
                    break;
                case TYPE_FEATURE.FILTER_SELECTED_ITEM_EXECUTE:
                    #region
                    if (ui_imageOrigin.Image != null && para != null)
                    {
                        Bitmap img = ui_imageOrigin.Image as Bitmap;
                        int w = ui_panelImages.Width / 3 - 30;
                        int h = w * 4 / 6;
                        string filter = para as string;

                        object config = null;
                        Bitmap image = ___IMAGE_FILTERS.Execute(filter, config, img);

                        //ui_panelImages.Controls.Add(new Label()
                        //{
                        //    Width = IMG_WIDTH,
                        //    Text = filter,
                        //    Height = 16,
                        //    RightToLeft = RightToLeft.No
                        //});

                        PictureBox p = new PictureBox() { Width = w, Height = h };
                        p.Image = image;
                        p.SizeMode = PictureBoxSizeMode.StretchImage;
                        ui_panelImages.Controls.Add(p);

                        //ui_panelImages.Controls.Add(new Label() { Width = IMG_WIDTH, Text = "", Height = 7 });

                        ////p.Click += (se, ev) => f_execute_features(TYPE_FEATURE.IMAGE_ORIGIN_SELECTED_ITEM, file);
                        //Thread.Sleep(100); 
                    }

                    #endregion
                    break;
                case TYPE_FEATURE.FILTER_INSERT_INTO_SCRIPT:
                    break;
                case TYPE_FEATURE.FILTER_EXECUTE_AUTO_CHANGED:
                    if (para != null)
                    {
                        FILTER_EXECUTE_AUTO_CHANGED = !FILTER_EXECUTE_AUTO_CHANGED;
                        (para as ToolStripMenuItem).Checked = FILTER_EXECUTE_AUTO_CHANGED;
                    }
                    break;
            }

            return null;
        }

    }
}
