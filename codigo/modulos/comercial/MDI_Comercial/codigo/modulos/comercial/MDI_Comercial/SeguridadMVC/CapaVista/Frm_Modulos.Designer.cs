
namespace Capa_Vista_Comercial
{
    partial class Frm_Modulos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Modulos));
            this.panelBarraSuperior = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCerrar = new System.Windows.Forms.PictureBox();
            this.btnMinimizar = new System.Windows.Forms.PictureBox();
            this.btnComercial = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnSeguridad = new System.Windows.Forms.Button();
            this.panelBarraSuperior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCerrar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimizar)).BeginInit();
            this.SuspendLayout();
            // 
            // panelBarraSuperior
            // 
            this.panelBarraSuperior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panelBarraSuperior.Controls.Add(this.btnCerrar);
            this.panelBarraSuperior.Controls.Add(this.btnMinimizar);
            this.panelBarraSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBarraSuperior.Location = new System.Drawing.Point(0, 0);
            this.panelBarraSuperior.Margin = new System.Windows.Forms.Padding(2);
            this.panelBarraSuperior.Name = "panelBarraSuperior";
            this.panelBarraSuperior.Size = new System.Drawing.Size(233, 43);
            this.panelBarraSuperior.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 48);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(226, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Image = ((System.Drawing.Image)(resources.GetObject("btnCerrar.Image")));
            this.btnCerrar.Location = new System.Drawing.Point(212, 7);
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(2);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(14, 16);
            this.btnCerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnCerrar.TabIndex = 1;
            this.btnCerrar.TabStop = false;
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Image = ((System.Drawing.Image)(resources.GetObject("btnMinimizar.Image")));
            this.btnMinimizar.Location = new System.Drawing.Point(195, 7);
            this.btnMinimizar.Margin = new System.Windows.Forms.Padding(2);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(14, 16);
            this.btnMinimizar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnMinimizar.TabIndex = 0;
            this.btnMinimizar.TabStop = false;
            // 
            // btnComercial
            // 
            this.btnComercial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComercial.Location = new System.Drawing.Point(0, 178);
            this.btnComercial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnComercial.Name = "btnComercial";
            this.btnComercial.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnComercial.Size = new System.Drawing.Size(226, 62);
            this.btnComercial.TabIndex = 4;
            this.btnComercial.Text = "Modulo Comercial";
            this.btnComercial.UseVisualStyleBackColor = true;
            this.btnComercial.Click += new System.EventHandler(this.btnComercial_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.Location = new System.Drawing.Point(-5, 246);
            this.btnSalir.Margin = new System.Windows.Forms.Padding(2);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(231, 55);
            this.btnSalir.TabIndex = 5;
            this.btnSalir.Text = "Salir del Sistema";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnSeguridad
            // 
            this.btnSeguridad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSeguridad.Image = ((System.Drawing.Image)(resources.GetObject("btnSeguridad.Image")));
            this.btnSeguridad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSeguridad.Location = new System.Drawing.Point(0, 112);
            this.btnSeguridad.Margin = new System.Windows.Forms.Padding(2);
            this.btnSeguridad.Name = "btnSeguridad";
            this.btnSeguridad.Size = new System.Drawing.Size(226, 60);
            this.btnSeguridad.TabIndex = 6;
            this.btnSeguridad.Text = "Modulo de Seguridad";
            this.btnSeguridad.UseVisualStyleBackColor = true;
            this.btnSeguridad.Click += new System.EventHandler(this.btnSeguridad_Click);
            // 
            // Frm_Modulos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 507);
            this.Controls.Add(this.btnSeguridad);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnComercial);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panelBarraSuperior);
            this.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_Modulos";
            this.Text = "Frm_Modulos";
            this.panelBarraSuperior.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCerrar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimizar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBarraSuperior;
        private System.Windows.Forms.PictureBox btnCerrar;
        private System.Windows.Forms.PictureBox btnMinimizar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnComercial;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnSeguridad;
    }
}