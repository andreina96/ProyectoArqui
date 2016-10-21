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
    public partial class Informacion : Form
    {
        /* Controladora para la ejecución del programa */
        Controladora con;
        Hilo h;

        public Informacion()
        {
            con = new Controladora();
            h = new Hilo();
            InitializeComponent();
        }

        /* Cuando se le da click al botón de directorio, entonces se carga el folderBrowser para obtener la carpeta 
         * en donde están los archivos */
        private void directorio_Click(object sender, EventArgs e)
        {
            folderBrowser.ShowDialog();
        }

        private void aceptar_Click(object sender, EventArgs e)
        {
            try
            {
                /* Los valores que se obtienen de esta pantalla, deben ser
                 * pasados a las clases correspondientes */
                if (Regex.IsMatch(numeroHilillos.Text, @"^[0-9\p{L} \w]+$"))
                {
                    if (Convert.ToInt32(numeroHilillos.Text) > 0)
                    {
                        /* Se asigna el valor al número de hilillos */
                        h.setNumeroHilillos(Convert.ToInt32(numeroHilillos.Text));
                        String[] archivos;
                        /* Si el directorio dado existe, se obtienen todos los nombres de los archivos que 
                         * tendrán hilillos */
                        if (Directory.Exists(folderBrowser.SelectedPath))
                        {
                            archivos = Directory.GetFiles(folderBrowser.SelectedPath, @"*.txt", SearchOption.TopDirectoryOnly);
                            int indice = 0;
                            while (indice < archivos.Length)
                            {
                                /* Se lee cada uno de los archivos y se obtienen las intrucciones de todos los hilillos */
                                Archivo a = new Archivo();
                                a.setRuta(archivos[indice]);
                                instruccion[] instrucciones = a.leerArchivo();
                                h.cargarInstruccionesMemoria(instrucciones, a.getNumInstrucciones());                                
                                indice++;
                            }
                            if (Regex.IsMatch(numeroQuantum.Text, @"^[0-9\p{L} \w]+$"))
                            {
                                if (Convert.ToInt32(numeroQuantum.Text) > 0)
                                {
                                    /* Se asigna el valor al número de quantum */
                                    h.setNumeroQuantum(Convert.ToInt32(numeroQuantum.Text));
                                }
                                else
                                {
                                    MessageBox.Show("El número de quantum debe ser mayor a 0!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("El valor del quantum debe ser un número!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("El directorio dado no es válido!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("El número de hilillos debe ser mayor a 0!");
                    }
                    /* Si se ingresan todos los datos de manera adecuada, se abre la pantalla de los resultados */
                    Resultados result = new Resultados();
                    result.Show();
                }
                else
                {
                    MessageBox.Show("El valor del número de hilillos debe ser un número!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error: " + ex);
            }
        }
    }
}
