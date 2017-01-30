using System;
using System.Collections.Generic;
using System.Text;

namespace HackAssembler
{
    /// <summary>
    /// Basic implementation of ICodeGenerator
    /// </summary>
    public class BasicCodeGenerator : ICodeGenerator
    {
        /// <summary>
        /// Implements method from ICodeGenerator
        /// </summary>
        /// <param name="commands"></param>
        /// <returns>List of instructions in binary text format</returns>
        public string Generate(List<string> commands)
        {
            var machineCodeBuilder = new StringBuilder();

            // Generates A and C instructions
            foreach (string command in commands)
            {
                if(command.StartsWith("@"))
                {
                    machineCodeBuilder.AppendLine(GetAInstruction(command));
                }
                else
                {
                    machineCodeBuilder.AppendLine(GetCInstruction(command));
                }
            }

            return machineCodeBuilder.ToString();
        }

        /// <summary>
        /// Translates A instructions to binary format
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Binary string</returns>
        private string GetAInstruction(string command)
        {
            int decimalNumber;
            command = command.TrimStart('@');

            // Tries to parse the instruction into a decimal integer since A instructions should only contain a number referring to a memory location
            if (!int.TryParse(command, out decimalNumber))
            {
                throw new Exception("Invalid A instruction");
            }

            // Converts the number to binary format
            var binaryNumber = Convert.ToString(decimalNumber, 2);

            // Pads with zeros to get 16 digits
            binaryNumber = binaryNumber.PadLeft(16, '0');

            return binaryNumber;
          }

        /// <summary>
        /// Translates C instructions to binary format
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Binary string</returns>
        private string GetCInstruction(string command)
        {
            var instructionBuilder = new StringBuilder();

            // Starts the instruction with three ones according to the hack specification
            instructionBuilder.Append("111");

            // Creates strings to store the destination, computation and jump fields of the instruction
            var dest = "";
            var comp = "";
            var jmp = "";

            // Gets the dest field from the left hand side of the equals sign if found
            if (command.Contains("="))
            {
                dest = command.Split('=')[0];
                comp = command.Split('=')[1];
            }
            else
            {
                comp = command;
            }

            // Gets the jmp field from the right hand side of the semicolon if found
            if(command.Contains(";"))
            {
                comp = command.Split(';')[0];
                jmp = command.Split(';')[1];
            }
            
            // Builds the binary string according to the hack specification
            if (!comp.Contains("M"))
            {
                instructionBuilder.Append("0");

                switch (comp)
                {
                    case "0":
                        {
                            instructionBuilder.Append("101010");
                            break;
                        }
                    case "1":
                        {
                            instructionBuilder.Append("111111");
                            break;
                        }
                    case "-1":
                        {
                            instructionBuilder.Append("111010");
                            break;
                        }
                    case "D":
                        {
                            instructionBuilder.Append("001100");
                            break;
                        }
                    case "A":
                        {
                            instructionBuilder.Append("110000");
                            break;
                        }
                    case "!D":
                        {
                            instructionBuilder.Append("001101");
                            break;
                        }
                    case "!A":
                        {
                            instructionBuilder.Append("110001");
                            break;
                        }
                    case "-D":
                        {
                            instructionBuilder.Append("001111");
                            break;
                        }
                    case "-A":
                        {
                            instructionBuilder.Append("110011");
                            break;
                        }
                    case "D+1":
                        {
                            instructionBuilder.Append("011111");
                            break;
                        }
                    case "A+1":
                        {
                            instructionBuilder.Append("110111");
                            break;
                        }
                    case "D-1":
                        {
                            instructionBuilder.Append("001110");
                            break;
                        }
                    case "A-1":
                        {
                            instructionBuilder.Append("110010");
                            break;
                        }
                    case "D+A":
                        {
                            instructionBuilder.Append("000010");
                            break;
                        }
                    case "D-A":
                        {
                            instructionBuilder.Append("010011");
                            break;
                        }
                    case "A-D":
                        {
                            instructionBuilder.Append("000111");
                            break;
                        }
                    case "D&A":
                        {
                            instructionBuilder.Append("000000");
                            break;
                        }
                    case "D|A":
                        {
                            instructionBuilder.Append("010101");
                            break;
                        }
                }
            }
            else
            {
                instructionBuilder.Append("1");

                switch (comp)
                {
                    case "M":
                        {
                            instructionBuilder.Append("110000");
                            break;
                        }
                    case "!M":
                        {
                            instructionBuilder.Append("110001");
                            break;
                        }
                    case "-M":
                        {
                            instructionBuilder.Append("110011");
                            break;
                        }
                    case "M+1":
                        {
                            instructionBuilder.Append("110111");
                            break;
                        }
                    case "M-1":
                        {
                            instructionBuilder.Append("110010");
                            break;
                        }
                    case "D+M":
                        {
                            instructionBuilder.Append("000010");
                            break;
                        }
                    case "D-M":
                        {
                            instructionBuilder.Append("010011");
                            break;
                        }
                    case "M-D":
                        {
                            instructionBuilder.Append("000111");
                            break;
                        }
                    case "D&M":
                        {
                            instructionBuilder.Append("000000");
                            break;
                        }
                    case "D|M":
                        {
                            instructionBuilder.Append("010101");
                            break;
                        }
                }
            }

            switch (dest)
            {
                case "M":
                    {
                        instructionBuilder.Append("001");
                        break;
                    }
                case "D":
                    {
                        instructionBuilder.Append("010");
                        break;
                    }
                case "MD":
                    {
                        instructionBuilder.Append("011");
                        break;
                    }
                case "A":
                    {
                        instructionBuilder.Append("100");
                        break;
                    }
                case "AM":
                    {
                        instructionBuilder.Append("101");
                        break;
                    }
                case "AD":
                    {
                        instructionBuilder.Append("110");
                        break;
                    }
                case "AMD":
                    {
                        instructionBuilder.Append("111");
                        break;
                    }
                default:
                    {
                        // The dest field can also be empty
                        instructionBuilder.Append("000");
                        break;
                    }
            }

            switch (jmp)
            {
                case "JGT":
                    {
                        instructionBuilder.Append("001");
                        break;
                    }
                case "JEQ":
                    {
                        instructionBuilder.Append("010");
                        break;
                    }
                case "JGE":
                    {
                        instructionBuilder.Append("011");
                        break;
                    }
                case "JLT":
                    {
                        instructionBuilder.Append("100");
                        break;
                    }
                case "JNE":
                    {
                        instructionBuilder.Append("101");
                        break;
                    }
                case "JLE":
                    {
                        instructionBuilder.Append("110");
                        break;
                    }
                case "JMP":
                    {
                        instructionBuilder.Append("111");
                        break;
                    }
                default:
                    {
                        // The jmp field can also be empty
                        instructionBuilder.Append("000");
                        break;
                    }
            }

            return instructionBuilder.ToString();
        }
    }
}