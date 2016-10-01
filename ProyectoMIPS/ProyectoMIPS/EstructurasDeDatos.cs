using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMIPS
{
    class EstructurasDeDatos
    {
        /* ======================================================
         * Se crean los elementos necesarios para crear las caché 
         * ====================================================== */

        /* Struct para representar una instrucción, compuesto por 4 enteros */
        public struct instruccion
        {
            public int entero1;
            public int entero2;
            public int entero3;
            public int entero4;
        }

        /* Struct para representar un bloque de datos, compuesto por 4 datos
         * y un bit de validez de bloque */
        public struct bloqueDatos
        {
            public int dato1;
            public int dato2;
            public int dato3;
            public int dato4;
            public bool validez;
        }

        /* Struct para representar un bloque de instrucciones, comuesto por 4 instrucciones 
         * y un bit de validez de bloque */
        public struct bloqueInstrucciones
        {
            public instruccion instruccion1;
            public instruccion instruccion2;
            public instruccion instruccion3;
            public instruccion instruccion4;
            public bool validez;
        }

        /* Struct para representar la caché de datos, compuesta por cuatro bloques de datos */
        public struct cacheDatos
        {
            public bloqueDatos bloqueDatos1;
            public bloqueDatos bloqueDatos2;
            public bloqueDatos bloqueDatos3;
            public bloqueDatos bloqueDatos4;
        }

        /* Struct para representar la caché de instrucciones, compuesta por cuatro bloques de instrucciones */
        public struct cacheInstrucciones
        {
            public bloqueInstrucciones bloqueInstrucciones1;
            public bloqueInstrucciones bloqueInstrucciones2;
            public bloqueInstrucciones bloqueInstrucciones3;
            public bloqueInstrucciones bloqueInstrucciones4;
        }

        /* ====================================================== 
         * ====================================================== */

        /* ======================================================
         * Se crea una estructura para representar los contextos
         * ====================================================== */

        public int[] contexto = new int[34];
    }
}
