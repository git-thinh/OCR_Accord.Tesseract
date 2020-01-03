using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ImageOcrExplorer
{
	public partial class TestFormIconFont : Form
	{
		public TestFormIconFont()
		{
			InitializeComponent();

			var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
			this.Text = versionInfo.ProductName;

			btnForeColor.BackColor = FontAwesome.DefaultProperties.ForeColor;
			btnForeColor.ForeColor = Invert(btnForeColor.BackColor);
			btnBackColor.BackColor = FontAwesome.DefaultProperties.BackColor;
			btnBackColor.ForeColor = Invert(btnBackColor.BackColor);
			btnBorderColor.BackColor = FontAwesome.DefaultProperties.BorderColor;
			btnBorderColor.ForeColor = Invert(btnBorderColor.BackColor);
			chkShowBorder.Checked = FontAwesome.DefaultProperties.ShowBorder;
			numericUpDown1.Value = FontAwesome.DefaultProperties.Size;

			//get list of fontawesome types
			comboBox1.Sorted = true;
			comboBox1.DataSource = Enum.GetNames(typeof(FontAwesome.Type));

			//quickest way to get image (with default properties)
			button1.Image = FontAwesome.Type.Crosshairs.AsImage();

			//stack multiple images together to one
			button2.Image = new FontAwesome.Properties(FontAwesome.Type.Square) { ForeColor = Color.White }.AsImage()
				.StackWith(new FontAwesome.Properties(FontAwesome.Type.FileO) { Size = 20, Location = new Point(5, 5), ShowBorder = false })
				.StackWith(new FontAwesome.Properties(FontAwesome.Type.Close) { ForeColor = Color.Red, Size = 14, Location = new Point(13, 13), ShowBorder = false });

			button3.Image = new FontAwesome.Properties(FontAwesome.Type.Save) { ForeColor = Color.Green, Size = button3.Height - 4, ShowBorder = false }.AsImage();

			//sample of diferent size
			pictureBox1.Image = FontAwesome.Instance.GetImage(new FontAwesome.Properties(FontAwesome.Type.Anchor) { ForeColor = Color.Blue, Size = 64, BackColor = Color.White });

			//icon is possible too
			this.Icon = new FontAwesome.Properties(FontAwesome.Type.Home) { ForeColor = Color.Red, BorderColor = Color.Red, BackColor = Color.White }.AsIcon();
		}

		private void UpdateColors(object sender)
		{
			var btn = sender as Button;
			if (btn != null)
			{
				colorDialog1.Color = btn.BackColor;
				if (colorDialog1.ShowDialog() == DialogResult.OK)
				{
					btn.BackColor = colorDialog1.Color;
					btn.ForeColor = Invert(btn.BackColor);
				}
			}
		}

		private Color Invert(Color color)
		{
			//for midgray set black as inverted color, to maintain visibility
			if ((color.R == 127 || color.R == 128) && (color.R == color.G && color.G == color.B))
			{
				return Color.Black;
			}
			return Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
		}

		private void btnForeColor_Click(object sender, EventArgs e)
		{
			UpdateColors(sender);
		}

		private void btnBackColor_Click(object sender, EventArgs e)
		{
			UpdateColors(sender);
		}

		private void btnBorderColor_Click(object sender, EventArgs e)
		{
			UpdateColors(sender);
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			var type = FontAwesome.ParseType(Convert.ToString(comboBox1.SelectedItem));
			pictureBox1.Image = FontAwesome.Instance.GetImage(
				new FontAwesome.Properties(type)
				{
					ForeColor = btnForeColor.BackColor,
					Size = (int)numericUpDown1.Value,
					BackColor = btnBackColor.BackColor,
					BorderColor = btnBorderColor.BackColor,
					ShowBorder = chkShowBorder.Checked
				});
		}


	}
}
