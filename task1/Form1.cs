using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions; // ضروري لفحص الإيميل
using System.IO; // ضروري للحفظ والتحميل

namespace task1
{
    public partial class Form1 : Form
    {
        // ------------------------------------------
        //  التصليح الأول: هنا كان اسم الدالة غلط
        // ------------------------------------------
        public Form1()
        {
            InitializeComponent();
        }

        // --- دالة زر التسجيل (مع التحقق) ---
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // (بقية الكود زي ما هو) ...
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("الاسم مطلوب!", "خطأ في التحقق",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("ادخل بريد إلكتروني صحيح!", "خطأ في التحقق",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("كلمة السر لازم تكون 6 أحرف أو أكثر!",
                "خطأ في التحقق", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (!rdoMale.Checked && !rdoFemale.Checked)
            {
                MessageBox.Show("الرجاء اختيار الجنس!", "خطأ في التحقق",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCountry.SelectedItem == null)
            {
                MessageBox.Show("الرجاء اختيار الدولة!", "خطأ في التحقق",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCountry.Focus();
                return;
            }

            if (lblSelectedColor.Text == "لم يتم اختيار اللون")
            {
                MessageBox.Show("الرجاء اختيار اللون المفضل!",
                "خطأ في التحقق", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // All validations passed
            string name = txtName.Text;
            string email = txtEmail.Text;
            string gender = rdoMale.Checked ? "ذكر" : "أنثى";
            string birthdate = dtpBirthdate.Value.ToShortDateString();
            string country = cmbCountry.SelectedItem.ToString();

            // ------------------------------------------
            //  التصليح الثاني: هنا كان النص الإنجليزي غلط
            // ------------------------------------------
            string color = lblSelectedColor.Text.Replace("اللون المختار: ", "");

            // Displaying result
            lblResult.Text = $"الاسم: {name}\nالإيميل: {email}\nالجنس: {gender}\n" +
                             $"تاريخ الميلاد: {birthdate}\nالدولة: {country}\nاللون المفضل: {color}";
        }

        // --- دالة زر اختيار اللون ---
        private void btnPickColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblSelectedColor.Text = $"اللون المختار: {colorDialog.Color.Name}";
            }
        }

        // --- دالة زر إعادة التعيين (النسخة النهائية) ---
        private void btnReset_Click(object sender, EventArgs e)
        {
            // (الكود سليم 100%)
            txtName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            rdoMale.Checked = false;
            rdoFemale.Checked = false;
            cmbCountry.SelectedIndex = -1;
            dtpBirthdate.Value = DateTime.Now;
            lblSelectedColor.Text = "لم يتم اختيار اللون";
            lblResult.Text = "النتيجة";
            picStudent.Image = null;
            txtName.Focus();
        }

        // --- دالة زر رفع الصورة ---
        private void btnUpload_Click(object sender, EventArgs e)
        {
            // (الكود سليم 100%)
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ملفات الصور|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                picStudent.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        // --- دالة زر حفظ البيانات ---
        private void btnSave_Click(object sender, EventArgs e)
        {
            // (الكود سليم 100%)
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("الرجاء تعبئة الاسم والإيميل قبل الحفظ!", "خطأ في التحقق", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                return;
            }

            string gender = rdoMale.Checked ? "Male" : (rdoFemale.Checked ? "Female" : "N/A");
            string data = $"{txtName.Text}\n" +          // Name
                          $"{txtEmail.Text}\n" +         // Email
                          $"{txtPassword.Text}\n" +      // Password
                          $"{gender}\n" +                // Gender
                          $"{dtpBirthdate.Value.ToShortDateString()}\n" + // Birthdate
                          $"{cmbCountry.SelectedItem?.ToString()}\n" + // Country
                          $"{lblSelectedColor.Text.Replace("اللون المختار: ", "").Replace("لم يتم اختيار اللون", "")}\n" + // Favorite Color
                          $"{(picStudent.Image != null ? "student_picture.jpg" : "NoImage")}\n"; // Image Path

            File.WriteAllText("student_data.txt", data);

            if (picStudent.Image != null)
            {
                picStudent.Image.Save("student_picture.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            MessageBox.Show("تم حفظ البيانات بنجاح!", "نجاح",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- دالة زر تحميل البيانات ---
        private void btnLoad_Click(object sender, EventArgs e)
        {
            // (الكود سليم 100%)
            if (!File.Exists("student_data.txt"))
            {
                MessageBox.Show("لا يوجد ملف بيانات محفوظ!", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] lines = File.ReadAllLines("student_data.txt");

            if (lines.Length < 8)
            {
                MessageBox.Show("ملف البيانات تالف أو غير مكتمل!", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtName.Text = lines[0];
            txtEmail.Text = lines[1];
            txtPassword.Text = lines[2];

            if (lines[3] == "Male") rdoMale.Checked = true;
            else if (lines[3] == "Female") rdoFemale.Checked = true;

            if (DateTime.TryParse(lines[4], out DateTime birthDate))
            {
                dtpBirthdate.Value = birthDate;
            }

            cmbCountry.SelectedItem = lines[5];

            if (!string.IsNullOrEmpty(lines[6]))
            {
                lblSelectedColor.Text = "اللون المختار: " + lines[6];
            }
            else
            {
                lblSelectedColor.Text = "لم يتم اختيار اللون";
            }

            if (File.Exists("student_picture.jpg") && lines[7] == "student_picture.jpg")
            {
                using (var bmpTemp = new Bitmap("student_picture.jpg"))
                {
                    picStudent.Image = new Bitmap(bmpTemp);
                }
            }
            else
            {
                picStudent.Image = null;
            }

            MessageBox.Show("تم تحميل البيانات بنجاح!", "نجاح",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}