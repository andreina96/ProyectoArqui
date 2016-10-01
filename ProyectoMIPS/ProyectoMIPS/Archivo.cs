using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ProyectoMIPS.EstructurasDeDatos;

namespace ProyectoMIPS
{
    class Archivo
    {
        string ruta;
        int numInstrucciones;

        public Archivo(string r)
        {
            ruta = r;
            numInstrucciones = 0;
        }

        /* Recorre el archivo, guardando en un arreglo todas las instrucciones
         * del hilillo */
        public instruccion[] leerArchivo()
        { 
            /* El arreglo se hace para 160 instrucciones, porque es el máximo que se
             * puede guardar en memoria */
            instruccion[] instrucciones = new instruccion[160];
            using (StreamReader sr = File.OpenText(ruta))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] p = linea.Split(' ');
                    try
                    {
                        instrucciones[numInstrucciones].entero1 = Convert.ToInt32(p[0]);
                        instrucciones[numInstrucciones].entero2 = Convert.ToInt32(p[1]);
                        instrucciones[numInstrucciones].entero3 = Convert.ToInt32(p[2]);
                        instrucciones[numInstrucciones].entero4 = Convert.ToInt32(p[3]);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Ha ocurrido un error al leer el archivo: " + ex);
                    }

                    numInstrucciones++;
                }
            }
            numInstrucciones++;

            return instrucciones;
        }

        /* Devuelve el número de instrucciones del hilillo */
        public int getNumInstrucciones()
        {
            return numInstrucciones;
        }
    }
}
