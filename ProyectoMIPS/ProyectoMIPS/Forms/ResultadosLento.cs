using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoMIPS.Forms
{
    public partial class ResultadosLento : Form
    {
        nucleo[] nucleoHilo;
        cacheDatos[] cacheDatosHilo;
        int reloj;
        int[] memoria;

        public ResultadosLento()
        {
            InitializeComponent();
        }

        private void ResultadosLento_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            thilillo1.ReadOnly = true;
            thilillo2.ReadOnly = true;
            thilillo3.ReadOnly = true;

            treloj1.ReadOnly = true;
            treloj2.ReadOnly = true;
            treloj3.ReadOnly = true;

            tregistros1.ReadOnly = true;
            tregistros2.ReadOnly = true;
            tregistros3.ReadOnly = true;

            tcache11.ReadOnly = true; tcache12.ReadOnly = true; tcache13.ReadOnly = true; tcache14.ReadOnly = true;
            tcache21.ReadOnly = true; tcache22.ReadOnly = true; tcache23.ReadOnly = true; tcache24.ReadOnly = true;
            tcache31.ReadOnly = true; tcache32.ReadOnly = true; tcache33.ReadOnly = true; tcache34.ReadOnly = true;

            tmemoria.ReadOnly = true;

            llenarCampos();
        }

        public void llenarCampos()
        {
            thilillo1.Text = nucleoHilo[0].obtener_num_hilillo().ToString();
            thilillo2.Text = nucleoHilo[1].obtener_num_hilillo().ToString();
            thilillo3.Text = nucleoHilo[2].obtener_num_hilillo().ToString();
            
            treloj1.Text = reloj.ToString(); treloj2.Text = reloj.ToString(); treloj3.Text = reloj.ToString();

            /*
             * Se llenan los registros de cada hilo
             */
            string reg1 = "", reg2 = "", reg3 = "";

            for(int i = 0; i < 33; i++)
            {
                reg1 = reg1 + "R" + i + ": " + nucleoHilo[0].obtener_registro(i) + " \n";
                reg2 = reg2 + "R" + i + ": " + nucleoHilo[1].obtener_registro(i) + " \n";
                reg3 = reg3 + "R" + i + ": " + nucleoHilo[2].obtener_registro(i) + " \n";
            }

            tregistros1.Text = reg1; tregistros2.Text = reg2; tregistros3.Text = reg3;

            /*
             * Se llenan las cachés de cada hilo
             */
            tcache11.Text = cacheDatosHilo[0].getBloque(0).getDato(0) + " \n"
                   + cacheDatosHilo[0].getBloque(0).getDato(1) + " \n"
                   + cacheDatosHilo[0].getBloque(0).getDato(2) + " \n"
                   + cacheDatosHilo[0].getBloque(0).getDato(3) + " \n";
            if (cacheDatosHilo[0].getBloque(0).validez == false)
                tcache11.Text = tcache11.Text + "IV \n";
            else
                tcache11.Text = tcache11.Text + "V \n";
            tcache12.Text = cacheDatosHilo[0].getBloque(1).getDato(0) + " \n"
                   + cacheDatosHilo[0].getBloque(1).getDato(1) + " \n"
                   + cacheDatosHilo[0].getBloque(1).getDato(2) + " \n"
                   + cacheDatosHilo[0].getBloque(1).getDato(3) + " \n";
            if (cacheDatosHilo[0].getBloque(1).validez == false)
                tcache12.Text = tcache12.Text + "IV \n";
            else
                tcache12.Text = tcache12.Text + "V \n";
            tcache13.Text = cacheDatosHilo[0].getBloque(2).getDato(0) + " \n"
                   + cacheDatosHilo[0].getBloque(2).getDato(1) + " \n"
                   + cacheDatosHilo[0].getBloque(2).getDato(2) + " \n"
                   + cacheDatosHilo[0].getBloque(2).getDato(3) + " \n";
            if (cacheDatosHilo[0].getBloque(2).validez == false)
                tcache13.Text = tcache13.Text + "IV \n";
            else
                tcache13.Text = tcache13.Text + "V \n";
            tcache14.Text = cacheDatosHilo[0].getBloque(3).getDato(0) + " \n"
                   + cacheDatosHilo[0].getBloque(3).getDato(1) + " \n"
                   + cacheDatosHilo[0].getBloque(3).getDato(2) + " \n"
                   + cacheDatosHilo[0].getBloque(3).getDato(3) + " \n";
            if (cacheDatosHilo[0].getBloque(3).validez == false)
                tcache14.Text = tcache14.Text + "IV \n";
            else
                tcache14.Text = tcache14.Text + "V \n";

            tcache21.Text = cacheDatosHilo[1].getBloque(0).getDato(0) + " \n"
                   + cacheDatosHilo[1].getBloque(0).getDato(1) + " \n"
                   + cacheDatosHilo[1].getBloque(0).getDato(2) + " \n"
                   + cacheDatosHilo[1].getBloque(0).getDato(3) + " \n";
            if (cacheDatosHilo[1].getBloque(0).validez == false)
                tcache21.Text = tcache21.Text + "IV \n";
            else
                tcache21.Text = tcache21.Text + "V \n";
            tcache22.Text = cacheDatosHilo[1].getBloque(1).getDato(0) + " \n"
                   + cacheDatosHilo[1].getBloque(1).getDato(1) + " \n"
                   + cacheDatosHilo[1].getBloque(1).getDato(2) + " \n"
                   + cacheDatosHilo[1].getBloque(1).getDato(3) + " \n";
            if (cacheDatosHilo[1].getBloque(1).validez == false)
                tcache22.Text = tcache22.Text + "IV \n";
            else
                tcache22.Text = tcache22.Text + "V \n";
            tcache23.Text = cacheDatosHilo[1].getBloque(2).getDato(0) + " \n"
                   + cacheDatosHilo[1].getBloque(2).getDato(1) + " \n"
                   + cacheDatosHilo[1].getBloque(2).getDato(2) + " \n"
                   + cacheDatosHilo[1].getBloque(2).getDato(3) + " \n";
            if (cacheDatosHilo[1].getBloque(2).validez == false)
                tcache23.Text = tcache23.Text + "IV \n";
            else
                tcache23.Text = tcache23.Text + "V \n";
            tcache24.Text = cacheDatosHilo[1].getBloque(3).getDato(0) + " \n"
                   + cacheDatosHilo[1].getBloque(3).getDato(1) + " \n"
                   + cacheDatosHilo[1].getBloque(3).getDato(2) + " \n"
                   + cacheDatosHilo[1].getBloque(3).getDato(3) + " \n";
            if (cacheDatosHilo[1].getBloque(3).validez == false)
                tcache24.Text = tcache24.Text + "IV \n";
            else
                tcache24.Text = tcache24.Text + "V \n";

            tcache31.Text = cacheDatosHilo[2].getBloque(0).getDato(0) + " \n"
                   + cacheDatosHilo[2].getBloque(0).getDato(1) + " \n"
                   + cacheDatosHilo[2].getBloque(0).getDato(2) + " \n"
                   + cacheDatosHilo[2].getBloque(0).getDato(3) + " \n";
            if (cacheDatosHilo[2].getBloque(0).validez == false)
                tcache31.Text = tcache31.Text + "IV \n";
            else
                tcache31.Text = tcache31.Text + "V \n";
            tcache32.Text = cacheDatosHilo[2].getBloque(1).getDato(0) + " \n"
                   + cacheDatosHilo[2].getBloque(1).getDato(1) + " \n"
                   + cacheDatosHilo[2].getBloque(1).getDato(2) + " \n"
                   + cacheDatosHilo[2].getBloque(1).getDato(3) + " \n";
            if (cacheDatosHilo[2].getBloque(1).validez == false)
                tcache32.Text = tcache32.Text + "IV \n";
            else
                tcache32.Text = tcache32.Text + "V \n";
            tcache33.Text = cacheDatosHilo[2].getBloque(2).getDato(0) + " \n"
                   + cacheDatosHilo[2].getBloque(2).getDato(1) + " \n"
                   + cacheDatosHilo[2].getBloque(2).getDato(2) + " \n"
                   + cacheDatosHilo[2].getBloque(2).getDato(3) + " \n";
            if (cacheDatosHilo[2].getBloque(2).validez == false)
                tcache33.Text = tcache33.Text + "IV \n";
            else
                tcache33.Text = tcache33.Text + "V \n";
            tcache34.Text = cacheDatosHilo[2].getBloque(3).getDato(0) + " \n"
                   + cacheDatosHilo[2].getBloque(3).getDato(1) + " \n"
                   + cacheDatosHilo[2].getBloque(3).getDato(2) + " \n"
                   + cacheDatosHilo[2].getBloque(3).getDato(3) + " \n";
            if (cacheDatosHilo[2].getBloque(3).validez == false)
                tcache34.Text = tcache34.Text + "IV \n";
            else
                tcache34.Text = tcache34.Text + "V \n";

            /*
             * Se llena la memoria principal
             */
            string mem = "";

            for (int i = 0; i < 96; i++)
                mem = mem + memoria[i] + " ";

            tmemoria.Text = mem;
        }

        private void bsiguiente_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void asignar_nucleo_hilo(nucleo[] nh)
        {
            nucleoHilo = nh;
        }

        public void asignar_reloj(int r)
        {
            reloj = r;
        }

        public void asignar_memoria(int[] m)
        {
            memoria = m;
        }

        public void asignar_cache_datos_hilo(cacheDatos[] cd)
        {
            cacheDatosHilo = cd;
        }
    }
}
