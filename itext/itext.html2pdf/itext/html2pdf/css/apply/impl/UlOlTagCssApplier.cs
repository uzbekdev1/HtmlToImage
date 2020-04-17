/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using System.Collections.Generic;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Css;
using iText.Html2pdf.Css.Apply.Util;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.StyledXmlParser.Css.Util;
using iText.StyledXmlParser.Node;

namespace iText.Html2pdf.Css.Apply.Impl {
    /// <summary>
    /// <see cref="iText.Html2pdf.Css.Apply.ICssApplier"/>
    /// implementation for Ul en Ol elements.
    /// </summary>
    public class UlOlTagCssApplier : BlockCssApplier {
        /* (non-Javadoc)
        * @see com.itextpdf.html2pdf.css.apply.impl.BlockCssApplier#apply(com.itextpdf.html2pdf.attach.ProcessorContext, com.itextpdf.html2pdf.html.node.IStylesContainer, com.itextpdf.html2pdf.attach.ITagWorker)
        */
        public override void Apply(ProcessorContext context, IStylesContainer stylesContainer, ITagWorker tagWorker
            ) {
            if (!(tagWorker.GetElementResult() is List)) {
                return;
            }
            IDictionary<String, String> css = stylesContainer.GetStyles();
            List list = (List)tagWorker.GetElementResult();
            if (CssConstants.INSIDE.Equals(css.Get(CssConstants.LIST_STYLE_POSITION))) {
                list.SetProperty(Property.LIST_SYMBOL_POSITION, ListSymbolPosition.INSIDE);
            }
            else {
                list.SetProperty(Property.LIST_SYMBOL_POSITION, ListSymbolPosition.OUTSIDE);
            }
            ListStyleApplierUtil.ApplyListStyleTypeProperty(stylesContainer, css, context, list);
            ListStyleApplierUtil.ApplyListStyleImageProperty(css, context, list);
            base.Apply(context, stylesContainer, tagWorker);
            // process the padding considering the direction
            bool isRtl = BaseDirection.RIGHT_TO_LEFT.Equals(list.GetProperty<BaseDirection?>(Property.BASE_DIRECTION));
            if ((isRtl && !list.HasProperty(Property.PADDING_RIGHT)) || (!isRtl && !list.HasProperty(Property.PADDING_LEFT
                ))) {
                float em = CssUtils.ParseAbsoluteLength(css.Get(CssConstants.FONT_SIZE));
                float rem = context.GetCssContext().GetRootFontSize();
                UnitValue startPadding = CssUtils.ParseLengthValueToPt(css.Get(CssConstants.PADDING_INLINE_START), em, rem
                    );
                list.SetProperty(isRtl ? Property.PADDING_RIGHT : Property.PADDING_LEFT, startPadding);
            }
        }
    }
}