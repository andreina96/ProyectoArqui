using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoMIPS;

namespace ProyectoMIPS.Forms
{
    public partial class Resultados : Form
    {
        Queue<hilillo> hilillos;
        int[] memoriaDatos;

        public Resultados(Queue<hilillo> h, int[] md)
        {
            hilillos = h;
            memoriaDatos = md;
            
            InitializeComponent();
        }
        
        public void Resultados_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();

            tregistros1.ReadOnly = true;
            tregistros2.ReadOnly = true;
            tregistros3.ReadOnly = true;

            tmemoria.ReadOnly = true;

            chilillo1.Items.Add("Seleccione");
            chilillo2.Items.Add("Seleccione");
            chilillo3.Items.Add("Seleccione");

            chilillo1.SelectedIndex = 0;
            chilillo2.SelectedIndex = 0;
            chilillo3.SelectedIndex = 0;

            for (int i = 0; i < hilillos.Count; i++)
            {
                int numero_hilo = hilillos.ElementAt(i).obtener_numero_nucleo();
                int numero_hilillo = hilillos.ElementAt(i).obtener_numero_hil();

                if (numero_hilo == 0)
                    chilillo1.Items.Add(numero_hilillo);
                else if(numero_hilo == 1)
                    chilillo2.Items.Add(numero_hilillo);
                else if(numero_hilo == 2)
                    chilillo3.Items.Add(numero_hilillo);
            }

            string mem = "";

            for (int i = 0; i < 96; i++)
                mem = mem + " " + memoriaDatos[i];

            tmemoria.Text = mem;
        }

        public void chilillo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            hilillo auxiilar;
            string reg = "";
            if (chilillo1.SelectedIndex != 0)
            {
                for (int i = 0; i < hilillos.Count; i++)
                {
                    if (hilillos.ElementAt(i).obtener_numero_hil().ToString() == chilillo1.SelectedItem.ToString())
                    {
                        auxiilar = hilillos.ElementAt(i);

                        int[] registros = auxiilar.obtener_registros();

                        for (int j = 0; j < registros.Length; j++)
                            reg = reg + "R" + j + ": " + registros[j] + " \n";

                        i = hilillos.Count;
                    }
                }
            }

            tregistros1.Text = reg;
        }

        public void chilillo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chilillo2.SelectedIndex != 0)
            {
                hilillo auxiilar;
                for (int i = 0; i < hilillos.Count; i++)
                {
                    if (hilillos.ElementAt(i).obtener_numero_hil().ToString() == chilillo2.SelectedItem.ToString())
                    {
                        auxiilar = hilillos.ElementAt(i);

                        string reg = "";
                        int[] registros = auxiilar.obtener_registros();

                        for (int j = 0; j < registros.Length; j++)
                        {
                            reg = reg + "R" + j + ": " + registros[j] + "\n";
                        }

                        tregistros2.Text = reg;

                        i = hilillos.Count;
                    }
                }
            }
        }

        public void chilillo3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chilillo3.SelectedIndex != 0)
            {
                hilillo auxiilar;
                for (int i = 0; i < hilillos.Count; i++)
                {
                    if (hilillos.ElementAt(i).obtener_numero_hil().ToString() == chilillo3.SelectedItem.ToString())
                    {
                        auxiilar = hilillos.ElementAt(i);

                        string reg = "";
                        int[] registros = auxiilar.obtener_registros();

                        for (int j = 0; j < registros.Length; j++)
                        {
                            reg = reg + "R" + j + ": " + registros[j] + "\n";
                        }

                        tregistros3.Text = reg;

                        i = hilillos.Count;
                    }
                }
            }
        }
    }
}
