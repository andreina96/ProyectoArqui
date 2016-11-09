using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ProyectoMIPS
{
    /* ======================================================
     * Clase 1: Hilillo
     * Estructura de datos para mantener la información de 
     * los hilillos
     * ====================================================== */
    public class hilillo
    {
        int numero_hilillo;
        int inicio_hilillo;  // Corresponde al inicio del hilillo
        int fin_hilillo;     // Corresponde al final del hilillo
        bool finalizado;     // Bandera que indica si ya finalizó
        int[] registros;     // R0 - R31 y RL
        int PC;

        public hilillo(int numero_hil)
        {
            numero_hilillo = numero_hil;
            inicio_hilillo = 0;
            fin_hilillo = 0;
            registros = new int [34];
            finalizado = false;
            PC = 0;
        }

        public int obtener_numero_hil()
        {
            return numero_hilillo;
        }
        // Asigna el inicio del hilillo
        public void asignar_inicio_hilillo(int inicia)
        {
            inicio_hilillo = inicia;
        }
        // Retorna el inicio del hilillo
        public int obtener_inicio_hilillo()
        {
            return inicio_hilillo;
        }
        // Asigna el final del hilillo
        public void asignar_fin_hilillo(int termina)
        {
            fin_hilillo = termina;
        }
        // Obtener el fin del hilillo
        public int obtener_fin_hilillo()
        {
            return fin_hilillo;
        }
        // Se obtiene el contador del programa
        public int obtener_PC()
        {
            return PC;
        }
        // Asigna el contador de programa del hilillo
        public void asignar_PC(int contador_programa)
        {
            PC = contador_programa;
        }
         
        // Obtener el contexto
        public int[] obtener_registros()
        {
            return registros;
        }

        // Asignar contexto de un hilillo
        public void asignar_contexto( int contador_programa, int[] reg)
        {
            PC = contador_programa;

            for (int i = 0; i < 33; i++)
                registros[i] = reg[i]; 
        }

        // Asignar estado del hilillo
        public void asignar_finalizado()
        {
            finalizado = true;
        }

        // Obtener estado del hilillo
        public bool obtener_finalizado()
        {
            return finalizado;
        }
    }
    
    /* ======================================================
     * Clase 2: Estructura Nucleo
     *
     * Inicio y fin del hilillo actual 
     *
     * Contador de programa: PC
     * 
     * Registros de propósito general: 
     *      registro[0]-registro[31]
     * 
     * Registro RL:
     *      registro[32]
     * ====================================================== */
    public class nucleo
    {
        public int[] registro;
        public int PC;
        public int inicio_hilillo;
        public int fin_hilillo;

        public nucleo()
        {
            PC = 0;
            registro = new int[33];
            registro[0] = 0;
            inicio_hilillo = 0;
            fin_hilillo = 0;
        }

        public void asignar_inicio_hilillo(int inicio)
        {
            this.inicio_hilillo = inicio;
        }

        public int obtener_inicio_hilillo()
        {
            return this.inicio_hilillo;
        }

        public void asignar_fin_hilillo(int fin)
        {
            this.fin_hilillo = fin;
        }

        public int obtener_fin_hilillo()
        {
            return this.fin_hilillo;
        }

        public void asignar_registro (int reg, int pos)
        {
            registro[pos] = reg;
        }

        public int obtener_registro(int pos)
        {
            return registro[pos];
        }

        public int obtener_contador_programa()
        {
            return this.PC;
        }

        public void aumentar_contador_programa()
        {
            PC = PC + 4;
        } 

        public void copiar_registros (hilillo hil)
        {
            PC = hil.obtener_PC();

            for (int i = 1; i < 33; i++)
                registro[i - 1] = hil.obtener_registros()[i];
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
                datos[i] = nuevosDatos[i];
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
                instrucciones[i] = nuevasInstrucciones[i];
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
            numeroBloque = new int[4];

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

        public void invalidar(int numBloque)
        {
            bloqueDatos[numBloque].validez = false;
        }

        public void modificarPalabraBloque(int dato, int palabra, int numBloque)
        {
            bloqueDatos[numBloque].setDato(dato, palabra);
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
            numeroBloque = new int[4];

            for (int i = 0; i < 4; i++)
            {
                bloqueInstruccion[i] = new bloqueInstrucciones();
                numeroBloque[i] = -1;
            }
        }

        public int getNumeroBloque(int numBloque)
        {
            return numeroBloque[numBloque % 4];
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
            if (numBloque == numeroBloque[numBloque % 4])
                return true;
            return false;
        }

        public bool esValido(int numBloque)
        {
            return bloqueInstruccion[numBloque].validez;
        }
    }
}
