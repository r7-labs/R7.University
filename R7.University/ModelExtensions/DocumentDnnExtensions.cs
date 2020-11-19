using System;
using System.Collections.Generic;
using System.Linq;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class DocumentDnnExtensions
    {
        public static IEnumerable<IDocument> WherePublished (this IEnumerable<IDocument> documents, DateTime now, bool isEditable)
        {
            return documents.Where (d => isEditable || d.IsPublished (now));
        }
    }
}

