using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ProyectoMIPS.EstructurasDeDatos;

namespace ProyectoMIPS
{
    class Controladora
    {
        /*
         * Variables de la clase
         */
        
        /* Se crea la variable para almacenar el quantum */
        public int quantum;
        /* Se crea la memoria principal como un arrerglo de 736 enteros */
        public int[] memoriaPrincipal;
        /* Se crea la variable que indica hasta qué posición de memoria de datos se
         * encuentra ocupado */
        int posDatosActual;
        /* Se crea la variable que indica hasta qué posición de memoria de instrucciones se
         * encuentra ocupado */
        int posInstruccionesActual;
        /* Se crea la cache de datos para cada uno de los hilos principales */
        public cacheDatos cacheDatosHilo1;
        public cacheDatos cacheDatosHilo2;
        public cacheDatos cacheDatosHilo3;
        /* Se crea la cache de instrucciones para cada uno de los hilos principales*/
        public cacheInstrucciones cacheInstruccionesHilo1;
        public cacheInstrucciones cacheInstruccionesHilo2;
        public cacheInstrucciones cacheInstruccionesHilo3;

        /* Se inicializan las variables de la clase */
        public Controladora()
        {
            memoriaPrincipal = new int[736];
            posDatosActual = 0;
            posInstruccionesActual = 96;
        }

        public void cargarInstrucciones(instruccion[] instrucciones, int numInstrucciones)
        {
            if(numInstrucciones > 160)
            {
                MessageBox.Show("El número máximo de instrucciones es 160!");
            }
            if((posInstruccionesActual - 96 + numInstrucciones) > 640)
            {
                MessageBox.Show("No hay espacio suficiente para almacenar ese número de instrucciones!");
            }
            for(int i = 0; i < numInstrucciones; i++)
            {
                memoriaPrincipal[posInstruccionesActual] = instrucciones[i].entero1;
                memoriaPrincipal[posInstruccionesActual + 1] = instrucciones[i].entero2;
                memoriaPrincipal[posInstruccionesActual + 2] = instrucciones[i].entero3;
                memoriaPrincipal[posInstruccionesActual + 3] = instrucciones[i].entero4;
                posInstruccionesActual += 4;
            }
        }
    }
}
