﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HackAssembler
{
    /// <summary>
    /// Basic implementation of ICodeGenerator
    /// </summary>
    public class BasicCodeGenerator : ICodeGenerator
    {
        IInstructionFieldConverter m_converter;

        public BasicCodeGenerator(IInstructionFieldConverter converter)
        {
            m_converter = converter;
        }

        /// <summary>
        /// ICodeGenerator that handles A and C instructions
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
            var destValue = "null";
            var cmpValue = "";
            var jmpValue = "null";

            // Gets the dest field from the left hand side of the equals sign if found
            if (command.Contains("="))
            {
                destValue = command.Split('=')[0];
                cmpValue = command.Split('=')[1];
            }
            else
            {
                cmpValue = command;
            }

            // Splits the jmp field from the comp string if a semicolon is found
            if(command.Contains(";"))
            {
                cmpValue = command.Split(';')[0];
                jmpValue = command.Split(';')[1];
            }

            instructionBuilder.Append(m_converter.ConvertField(cmpValue, "cmp"));

            if (!string.IsNullOrEmpty(destValue))
            {
                instructionBuilder.Append(m_converter.ConvertField(destValue, "dest"));
            }

            if (!string.IsNullOrEmpty(jmpValue))
            {
                instructionBuilder.Append(m_converter.ConvertField(jmpValue, "jmp"));
            }
            
            return instructionBuilder.ToString();
        }
    }
}