using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ProyectoMIPS
{
    /* ======================================================
     * Estructura de datos para mantener la información de 
     * los hilillos
     * ====================================================== */

    public class hilillo
    {
        int iniciaHilillo;
        int terminaHilillo;
        contexto contextoHilillo;
        bool finalizado;

        public hilillo()
        {
            iniciaHilillo = 0;
            terminaHilillo = 0;
            contextoHilillo = new contexto();
            finalizado = false;
        }

        public void setHililloInicia(int inicia)
        {
            iniciaHilillo = inicia;
        }

        public int getHililloInicia()
        {
            return iniciaHilillo;
        }

        public void setHililloTermina(int termina)
        {
            terminaHilillo = termina;
        }

        public int getHililloTermina()
        {
            return terminaHilillo;
        }

        public contexto getContexto()
        {
            return contextoHilillo;
        }

        public void setFinalizado()
        {
            finalizado = true;
        }

        public bool getFinalizado()
        {
            return finalizado;
        }
    }
    
    /* ======================================================
     * Estructura de datos cola para guardar los contextos
     * de los hilos
     * 
     * Registros de propósito general: 
     *      registro[0]-registro[31]
     * 
     * Registro RL:
     *      registro[32]
     *      
     * Registro PC:
     *      registro[33] 
     * ====================================================== */

    public class contexto
    {
        public int[] registro;

        public contexto()
        {
            registro = new int[34];
            registro[0] = 0;
        }

        public void setRegistro(int r, int i)
        {
            registro[i] = r;
        }

        public int getRegistro(int i)
        {
            return registro[i];
        }

        public void copiar(contexto n)
        {
            for (int i = 1; i < 34; i++)
            {
                registro[i] = n.getRegistro(i);
            }
        }
    }

    /* ======================================================
    * Estructura de datos para almacenar una instrucción
    * 
    * Código de instrucción:
    *       instruc[0]
    *    
    * Primer operando:
    *       instruc[1]
    *       
    * Segundo operando:
    *       instruc[2]
    *       
    * Tercer operando:
    *       instruc[3]
    * ====================================================== */
    
    public class instruccion
    {
        int[] instruc;

        public instruccion()
        {
            instruc = new int[4];
        }

        public void setParteInstruccion(int parte, int indice)
        {
            instruc[indice] = parte;
        }

        public int getParteInstruccion(int indice)
        {
            return instruc[indice];
        }

        public int[] getInstruccion()
        {
            return instruc;
        }
    }

    /* ======================================================
    * Estructura de datos para almacenar un bloque de datos
    * 
    * 4 palabras del bloque:
    *       datos[0]-datos[3]
    * 
    * Bit de validez del bloque:
    *       validez (inicialmente falso)
    * ====================================================== */

    public class bloqueDatos
    {
        public int[] datos;
        public bool validez;

        public bloqueDatos()
        {
            datos = new int[4];
            validez = false;
        }

        public void setDato(int dato, int indice)
        {
            datos[indice] = dato;
            validez = true;
        }

        public int getDato(int indice)
        {
            return datos[indice];
        }

        public void setBloque(int[] nuevosDatos)
        {
            for (int i = 0; i < 4; i++)
            {
                datos[i] = nuevosDatos[i];
            }
        }

        public int[] getBloque()
        {
            return datos;
        }
    }

    /* ======================================================
    * Estructura de datos para almacenar un bloque de instru-
    * cciones
    * 
    * 4 instrucciones del bloque:
    *       instrucciones[0]-instrucciones[3]
    * 
    * Bit de validez del bloque:
    *       validez (inicialmente falso)
    * ====================================================== */

    public class bloqueInstrucciones
    {
        public instruccion[] instrucciones;
        public bool validez;

        public bloqueInstrucciones()
        {
            instrucciones = new instruccion[4];
            validez = false;

            for (int i = 0; i < 4; i++)
                instrucciones[i] = new instruccion();
        }

        public void setInstruccion(instruccion instruccion, int indice)
        {
            instrucciones[indice] = instruccion;
            validez = true;
        }

        public instruccion getInstruccion(int indice)
        {
            return instrucciones[indice];
        }

        public void setBloque(instruccion[] nuevasInstrucciones)
        {
            for (int i = 0; i < 4; i++)
            {
                instrucciones[i] = nuevasInstrucciones[i];
            }
        }

        public instruccion[] getBloque()
        {
            return instrucciones;
        }
    }

    /* ======================================================
    * Estructura de datos para almacenar una caché de datos
    * 
    * 4 bloques de datos:
    *       bloqueDatos[0]-bloqueDatos[3]
    * 
    * 4 números de bloque para cada bloque:
    *       numeroBloque[0]-numeroBloque[3] (inicialmente en
    *       -1)
    * ====================================================== */

    public class cacheDatos
    {
        public bloqueDatos[] bloqueDatos;
        public int[] numeroBloque;

        public cacheDatos()
        {
            bloqueDatos = new bloqueDatos[4];

            for (int i = 0; i < 4; i++)
            {
                bloqueDatos[i] = new bloqueDatos();
                numeroBloque[i] = -1;
            }
        }

        public void setBloque(int[] bloqueNuevo, int nuevoNumeroBloque)
        {
            bloqueDatos[nuevoNumeroBloque % 4].setBloque(bloqueNuevo);
            bloqueDatos[nuevoNumeroBloque % 4].validez = false;
            numeroBloque[nuevoNumeroBloque % 4] = nuevoNumeroBloque;
        }

        public bloqueDatos getBloque(int numBloque)
        {
            return bloqueDatos[numBloque % 4];
        }

        public bool esNumeroBloque(int numBloque)
        {
            if((numBloque % 4) == numeroBloque[numBloque % 4])
                return true;
            return false;
        }

        public bool esValido(int numBloque)
        {
            return bloqueDatos[numBloque].validez;
        }
    }

    /* ======================================================
    * Estructura de datos para almacenar una caché de instru-
    * cciones
    * 
    * 4 bloques de instrucciones:
    *       bloqueInstrucciones[0]-bloqueInstrucciones[3]
    * 
    * 4 números de bloque para cada bloque:
    *       numeroBloque[0]-numeroBloque[3] (inicialmente en
    *       -1)
    * ====================================================== */

    public class cacheInstrucciones
    {
        public bloqueInstrucciones[] bloqueInstruccion;
        public int[] numeroBloque;

        public cacheInstrucciones()
        {
            bloqueInstruccion = new bloqueInstrucciones[4];

            for (int i = 0; i < 4; i++)
            {
                bloqueInstruccion[i] = new bloqueInstrucciones();
                numeroBloque[i] = -1;
            }
        }

        public void setBloque(instruccion[] bloqueNuevo, int nuevoNumeroBloque)
        {
            bloqueInstruccion[nuevoNumeroBloque % 4].setBloque(bloqueNuevo);
            bloqueInstruccion[nuevoNumeroBloque % 4].validez = false;
            numeroBloque[nuevoNumeroBloque % 4] = nuevoNumeroBloque;
        }

        public bloqueInstrucciones getBloque(int numBloque)
        {
            return bloqueInstruccion[numBloque % 4];
        }

        public bool esNumeroBloque(int numBloque)
        {
            if ((numBloque % 4) == numeroBloque[numBloque % 4])
                return true;
            return false;
        }

        public bool esValido(int numBloque)
        {
            return bloqueInstruccion[numBloque].validez;
        }
    }
}
