using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoMIPS.Forms
{
    public partial class UserInformation : Form
    {
        public UserInformation()
        {
            InitializeComponent();
        }

        private void UserInformation_Load(object sender, EventArgs e)
        {

        }

        private void directorio_Click(object sender, EventArgs e)
        {
            folderBrowser.ShowDialog();
        }

        private void aceptar_Click(object sender, EventArgs e)
        {
            /* Los valores que se obtienen de esta pantalla, deben ser
             * pasados a las clases correspondientes */
            if (Convert.ToInt32(numeroHilillos.Text) > 0) { 
                // Aquí se le asigna a la variable hilillos el valor
                String[] archivos;
                /* Si el directorio dado existe, se obtienen todos los nombres de los archivos que 
                 * tendrán hilillos */
                if (Directory.Exists(folderBrowser.SelectedPath))
                {
                    archivos = Directory.GetFiles(folderBrowser.SelectedPath, @"*.txt", SearchOption.TopDirectoryOnly);
                    int indice = 0;
                    while (indice < archivos.Length)
                    {
                        /* Se imprimen todos los archivos, sólo para ver si funciona*/
                        MessageBox.Show(archivos[indice]);
                        indice++;
                    }
                    if (Convert.ToInt32(numeroQuantum.Text) > 0)
                    {
                        // Aquí se le asigna a la variable quantum el valor 
                    }
                    else
                    {
                        MessageBox.Show("El número de quantum debe ser mayor a 0!");
                    }
                }
                else
                {
                    MessageBox.Show("El directorio dado no es válido!");
                }
            }
            else
            {
                Console.WriteLine("El número de hilillos debe ser mayor a 0!");
            }
        }
    }
}
