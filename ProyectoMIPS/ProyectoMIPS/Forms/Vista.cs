using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoMIPS;

namespace ProyectoMIPS.Forms
{
    public partial class Vista : Form
    {
        Controladora controladora;
        int numero_hilillos;
        int quantum;
        String[] rutas_archivos;

        public Vista()
        {
            numero_hilillos = 0;
            quantum = 0;
            rutas_archivos = new String [8];
            InitializeComponent();
        }
        
        private void directorio_Click(object sender, EventArgs e)
        {
            folderBrowser.ShowDialog();
        }

        private void aceptar_Click(object sender, EventArgs e)
        {
            simulacion(false);
        }

        private void aceptarr_Click(object sender, EventArgs e)
        {
            simulacion(true);
        }

        public void simulacion(bool modo)
        {
            try
            {
                if (Regex.IsMatch(numeroHilillos.Text, @"^[0-9\p{L} \w]+$"))
                {
                    if (Convert.ToInt32(numeroHilillos.Text) > 0)
                    {
                        numero_hilillos = Convert.ToInt32(numeroHilillos.Text);

                        if (Directory.Exists(folderBrowser.SelectedPath))
                        {
                            rutas_archivos = Directory.GetFiles(folderBrowser.SelectedPath, @"*.txt", SearchOption.TopDirectoryOnly);

                            if (Regex.IsMatch(numeroQuantum.Text, @"^[0-9\p{L} \w]+$"))
                            {
                                if (Convert.ToInt32(numeroQuantum.Text) > 0)
                                {
                                    quantum = Convert.ToInt32(numeroQuantum.Text);
                                    controladora = new Controladora(numero_hilillos, quantum, rutas_archivos, modo);
                                    controladora.iniciarSimulacion();
                                }
                                else
                                    MessageBox.Show("El número de quantum debe ser mayor a 0!");
                            }
                            else
                                MessageBox.Show("El valor del quantum debe ser un número!");
                        }
                        else
                            MessageBox.Show("El directorio dado no es válido!");
                    }
                    else
                        MessageBox.Show("El número de hilillos debe ser mayor a 0!");
                }
                else
                    MessageBox.Show("El valor del número de hilillos debe ser un número!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error: " + ex);
            }
        }
    }
}
