using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpuSimulator
{
    public enum InstructionTyp : int
    {

        /*************************
         * Debugging Instructions
         *************************/

        /// <summary>
        /// Dump the Registers to Output
        /// </summary>
        RDUMP               = 10,

        /// <summary>
        /// enable fetch output (show next command to decode)
        /// </summary>
        TRACE_FETCH         = 11,

        /// <summary>
        ///  enable decode output (tell me what you want to do)
        /// </summary>
        TRACE_DECODE        = 12,

        /// <summary>
        /// Dump RAM Content to Output
        /// </summary>
        MDUMP               = 13,

        /// <summary>
        /// Dump Stack Content to Output
        /// </summary>
        SDUMP               = 14,


        /*************************
         * Processor Instructions
         *************************/

        /// <summary>
        /// MOV  Ziel, Quelle        kopiere Quelle nach Ziel        ///         Quelle:     Register     A .. H        ///                     Daten direkt      #nnn        ///                     RAM        @adr        ///                     SP-offset    $-offset        ///         Ziel:       Register    A .. H        ///                     RAM         @adr        ///Ziel und Quelle können nicht gleichzeitig Adresse beinhalten
        /// </summary>
        MOV                 = 20,

        /// <summary>
        /// MOVI  [Ziel], Quelle        kopiere Quelle nach Memory[Ziel],         ///                                indirekte Adressierung über Register         ///         Quelle:     Register    A .. H        ///                     Daten         #nnn        ///         Ziel:       Register    A .. H
        /// 
        /// MOVI Ziel, [Quelle]        kopiere Daten von Speicher[Quelle] nach Ziel        ///         Quelle:      Register    A .. H        ///         Ziel:        Register    A .. H
        /// </summary>
        MOVI                = 21,

        /// <summary>
        /// HALT                Programm-Ende, implizit RDUMP
        /// </summary>
        HALT                = 22,

        /// <summary>
        /// PUSH Quelle                 lege Quelle auf STACK, SP++        ///         Quelle:       Register    A .. H        ///                       Daten direkt    #nnn
        /// </summary>
        PUSH                = 23,

        /// <summary>
        /// POP Ziel             SP--, hole Daten vom Stack        ///         Ziel:        Register    A .. H
        /// </summary>
        POP                 = 24,

        /// <summary>
        /// CALL @adr            <PUSH PC; PC = adr>
        /// </summary>
        CALL                = 25,

        /// <summary>
        /// RET                <POP PC>
        /// </summary>
        RET                 = 26,


        /****************************
        * Jump (Sprung) Instructions
        *****************************/

        /// <summary>
        /// JMP @adr        gehe zu Adresse @adr, absolut        ///  <PC = adr>
        /// </summary>
        JMP                 = 30,

        /// <summary>
        /// JR  #nnn         springe relativ zur aktuellen Adresse        ///  <PC = PC + nnn>
        /// </summary>
        JR                  = 31,

        /// <summary>
        /// JRC  #nnn        springe relativ wenn C gesetzt ist
        /// </summary>
        JRC                 = 32,

        /// <summary>
        /// JRZ  #nnn        springe relativ wenn Z gesetzt ist
        /// </summary>
        JRZ                 = 33,

        /// <summary>
        /// JRN #nnn        springe relativ wenn N gesetzt ist
        /// </summary>
        JRN                 = 34,

        /*************************
         * ALU Instructions
         *************************/

        /// <summary>
        /// AND Reg1, Reg2        bitweises AND,  A = Reg1 AND Reg2        /// Flags: Z, N
        /// </summary>
        AND                 = 40,

        /// <summary>
        /// OR Reg1, Reg2            bitweises OR, A = Reg1 OR Reg2        /// Flags: Z, N
        /// </summary>
        OR                  = 41,

        /// <summary>
        /// XOR Reg1, Reg2        bitweises XOR, A = Reg1 XOR Reg2        /// Flags: Z, N
        /// </summary>
        XOR                 = 42,

        /// <summary>
        /// ADD Reg1, Reg2        Addition, A = Reg1 + Reg2        /// Flags: Z, N, C wenn Ergebnis Überlauf verursacht
        /// </summary>
        ADD                 = 43,

        /// <summary>
        /// SUB Reg1, Reg2        Subtraktion, A = Reg1 – Reg2        /// Flags: Z, N, C wenn Ergebnis Überlauf verursacht
        /// </summary>
        SUB                 = 44,

        /// <summary>
        /// SHR Reg1, Reg2            rechts Schieben, A = Reg1 >> Reg2        /// Flags: Z, N, C hält letztes rausgeschobenes Bit
        /// </summary>
        SHR                 = 45,

        /// <summary>
        /// SHL Reg1, Reg2            links Schieben, A = Reg1 << Reg2        /// Flags: Z, N, C hält letztes rausgeschobenes Bit
        /// </summary>
        SHL                 = 46,

        /// <summary>
        /// RR Reg1, Reg2            rotiere rechts        /// Flags: Z, N, C hält letztes rotiertes Bit (Result Bit 31)
        /// </summary>
        RR                  = 47,

        /// <summary>
        /// RL Reg1, Reg2            rotiere links                        /// Flags: Z, N, C hält letztes rotiertes Bit (Result Bit 0)
        /// </summary>
        RL                  = 48,

        /// <summary>
        /// RRC Reg1, Reg2            rotiere rechts über Carry        /// Flags: Z, N, C
        /// </summary>
        RRC                 = 49,

        /// <summary>
        /// RLC Reg1, Reg2            rotiere links über Carry        /// Flags: Z, N, C
        /// </summary>
        RLC                 = 50 


    }
}
