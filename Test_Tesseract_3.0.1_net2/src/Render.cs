using System;
using System.Collections.Generic;
using System.Text;
using OCR.TesseractWrapper;
using System.Drawing;

namespace IPoVn.OCRer
{
    internal class Render
    {
        public static void DrawBlock(Graphics grph, Block block)
        {
            foreach (Paragraph para in block.Paragraphs)
                DrawParagraph(grph, para);

            block.Draw(grph);
        }

        public static void DrawParagraph(Graphics grph, Paragraph para)
        {
            foreach (TextLine line in para.Lines)
                DrawTextLine(grph, line);

            para.Draw(grph);
        }

        public static void DrawTextLine(Graphics grph, TextLine line)
        {
            foreach (Word word in line.Words)
                DrawWord(grph, word);

            line.Draw(grph);
        }

        public static void DrawWord(Graphics grph, Word word)
        {
            foreach (Character ch in word.CharList)
                DrawChar(grph, ch);

            word.Draw(grph);
        }

        public static void DrawChar(Graphics grph, Character ch)
        {
            ch.Draw(grph);
        }
    }
}
