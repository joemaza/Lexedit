//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eutyches.Spell.Utilities
{
    internal static class Commenting
    {
        #region Fields

        /// <summary>
        /// The right edge column
        /// </summary>
        private const int RightEdgeColumn = 119;

        /// <summary>
        /// The comment bar
        /// </summary>
        private static string CommentBar = $"#{new string('=', RightEdgeColumn)}";

        /// <summary>
        /// The section bar
        /// </summary>
        private static string SectionBar = $"#{new string('~', RightEdgeColumn)}";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Adds the comment bar.
        /// </summary>
        /// <param name="lines">The lines.</param>
        public static void AddCommentBar(this IList<string> lines)
        {
            lines.Add(CommentBar);
        }

        /// <summary>
        /// Adds the comment bar.
        /// </summary>
        /// <param name="sb">The sb.</param>
        public static void AddCommentBar(this StringBuilder sb)
        {
            sb.AppendLine(CommentBar);
        }

        /// <summary>
        /// Adds the comment block.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="comments">The comments.</param>
        public static void AddCommentBlock(this StringBuilder sb, string comments)
        {
            sb.AddCommentBar();
            sb.AddComments(comments);
            sb.AddCommentBar();
        }

        /// <summary>
        /// Adds the comment block.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="comments">The comments.</param>
        public static void AddCommentBlock(this IList<string> lines, string comments)
        {
            lines.AddCommentBar();
            lines.AddComments(comments);
            lines.AddCommentBar();
        }

        /// <summary>
        /// Adds the comment block.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="comments">The comments.</param>
        public static void AddCommentBlock(this IList<string> lines, IEnumerable<string> comments)
        {
            lines.AddCommentBar();
            lines.AddComments(comments);
            lines.AddCommentBar();
        }

        /// <summary>
        /// Adds the comment block.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="comments">The comments.</param>
        public static void AddCommentBlock(this StringBuilder sb, IEnumerable<string> comments)
        {
            sb.AddCommentBar();
            sb.AddComments(comments);
            sb.AddCommentBar();
        }

        /// <summary>
        /// Adds the comment.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="comments">The comments.</param>
        public static void AddComments(this IList<string> lines, string comments)
        {
            AddComments(lines, comments.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
        }

        /// <summary>
        /// Adds the comment.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="comments">The comments.</param>
        public static void AddComments(this IList<string> lines, IEnumerable<string> comments)
        {
            foreach(var line in comments)
            {
                lines.Add($"# {line}");
            }
        }

        /// <summary>
        /// Adds the comments.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="comments">The comments.</param>
        public static void AddComments(this StringBuilder sb, string comments)
        {
            AddComments(sb, comments.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
        }

        /// <summary>
        /// Adds the comments.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="comments">The comments.</param>
        public static void AddComments(this StringBuilder sb, IEnumerable<string> comments)
        {
            foreach(var line in comments)
            {
                sb.AppendLine($"# {line}");
            }
        }

        /// <summary>
        /// Adds the section bar.
        /// </summary>
        /// <param name="lines">The lines.</param>
        public static void AddSectionBar(this IList<string> lines)
        {
            lines.Add(SectionBar);
        }

        /// <summary>
        /// Adds the section bar.
        /// </summary>
        /// <param name="sb">The sb.</param>
        public static void AddSectionBar(this StringBuilder sb)
        {
            sb.AppendLine(SectionBar);
        }

        /// <summary>
        /// Adds the section block.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="comments">The comments.</param>
        public static void AddSectionBlock(this IList<string> lines, string comments)
        {
            lines.AddSectionBar();
            lines.AddComments(comments);
            lines.AddSectionBar();
        }

        /// <summary>
        /// Adds the section block.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="comments">The comments.</param>
        public static void AddSectionBlock(this StringBuilder sb, string comments)
        {
            sb.AddSectionBar();
            sb.AddComments(comments);
            sb.AddSectionBar();
        }

        /// <summary>
        /// Adds the section block.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="comments">The comments.</param>
        public static void AddSectionBlock(this IList<string> lines, IEnumerable<string> comments)
        {
            lines.AddSectionBar();
            lines.AddComments(comments);
            lines.AddSectionBar();
        }

        /// <summary>
        /// Adds the section block.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="comments">The comments.</param>
        public static void AddSectionBlock(this StringBuilder sb, IEnumerable<string> comments)
        {
            sb.AddSectionBar();
            sb.AddComments(comments);
            sb.AddSectionBar();
        }

        #endregion Methods
    }
}