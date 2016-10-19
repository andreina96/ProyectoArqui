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

        /* Se crea la variable para el número de hilillos */
        public int numHilillos;

        /* Se crea la memoria principal como un arrerglo de 736 enteros */
        public int[] memoriaPrincipal;

        /* Se crea la variable que indica hasta qué posición de memoria de datos se
         * encuentra ocupado */
        public int posDatosActual;

        /* Se crea la variable que indica hasta qué posición de memoria de instrucciones se
         * encuentra ocupado */
        public int posInstruccionesActual;

        /* Se crea el contexto para cada uno de los hilos principales */
        public contexto[] cont;

        /* Se inicializan las variables de la clase */
        public Controladora()
        {
            quantum = 0;
            numHilillos = 0;
            memoriaPrincipal = new int[736];
            posDatosActual = 0;
            posInstruccionesActual = 96;
            cont = new contexto[3];
            cont[0] = new contexto();
            cont[1] = new contexto();
            cont[2] = new contexto();

            for (int i = 0; i < 736; i++)
                memoriaPrincipal[i] = 1;

            cacheDatosHilo1 = new cacheDatos();  // Se inicializan las caches de datos (Cache 1)
            cacheDatosHilo2 = new cacheDatos();  // Se inicializan las caches de datos (Cache 2)
            cacheDatosHilo3 = new cacheDatos();  // Se inicializan las caches de datos (Cache 3)

            cacheInstruccionesHilo1 = new cacheInstrucciones();   // Se inicializan las caches de instrucciones (Cache 1)
            cacheInstruccionesHilo2 = new cacheInstrucciones();   // Se inicializan las caches de instrucciones (Cache 2)
            cacheInstruccionesHilo3 = new cacheInstrucciones();   // Se inicializan las caches de instrucciones (Cache 3)

        }

        /* Carga las instrucciones en memoria */
        public void cargarInstrucciones(instruccion[] instrucciones, int numInstrucciones)
        {
            if (numInstrucciones > 160)
            {
                MessageBox.Show("El número máximo de instrucciones es 160!");
            }
            if ((posInstruccionesActual - 96 + numInstrucciones) > 640)
            {
                MessageBox.Show("No hay espacio suficiente para almacenar ese número de instrucciones!");
            }
            for (int i = 0; i < numInstrucciones; i++)
            {
                memoriaPrincipal[posInstruccionesActual] = instrucciones[i].entero[0];
                memoriaPrincipal[posInstruccionesActual + 1] = instrucciones[i].entero[1];
                memoriaPrincipal[posInstruccionesActual + 2] = instrucciones[i].entero[2];
                memoriaPrincipal[posInstruccionesActual + 3] = instrucciones[i].entero[3];
                posInstruccionesActual += 4;
            }
        }

        public bloqueInstrucciones getBloqueInstrucciones(int nb)
        {
            bloqueInstrucciones bi = new bloqueInstrucciones();
            int[] instruccion = new int[4];
            instruccion[0] = memoriaPrincipal[nb];
            instruccion[1] = memoriaPrincipal[nb+1];
            instruccion[2] = memoriaPrincipal[nb+2];
            instruccion[3] = memoriaPrincipal[nb+3];
            bi.setPalabra(instruccion, 0);
            nb = nb + 4;
            instruccion[0] = memoriaPrincipal[nb];
            instruccion[1] = memoriaPrincipal[nb + 1];
            instruccion[2] = memoriaPrincipal[nb + 2];
            instruccion[3] = memoriaPrincipal[nb + 3];
            bi.setPalabra(instruccion, 1);
            nb = nb + 4;
            instruccion[0] = memoriaPrincipal[nb];
            instruccion[1] = memoriaPrincipal[nb + 1];
            instruccion[2] = memoriaPrincipal[nb + 2];
            instruccion[3] = memoriaPrincipal[nb + 3];
            bi.setPalabra(instruccion, 2);
            nb = nb + 4;
            instruccion[0] = memoriaPrincipal[nb];
            instruccion[1] = memoriaPrincipal[nb + 1];
            instruccion[2] = memoriaPrincipal[nb + 2];
            instruccion[3] = memoriaPrincipal[nb + 3];
            bi.setPalabra(instruccion, 3);

            return bi;
        }
    }
}
