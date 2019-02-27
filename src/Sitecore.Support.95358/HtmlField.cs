using HtmlAgilityPack;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sitecore.Support.Data.Fields
{
  public class HtmlField : Sitecore.Data.Fields.HtmlField
  {
    static Type htmlFieldClassType = typeof(Sitecore.Data.Fields.HtmlField);
    static MethodInfo addMediaLinkMethod = htmlFieldClassType.GetMethod("AddMediaLink", BindingFlags.Instance | BindingFlags.NonPublic);

    public HtmlField(Field innerField)
            : base(innerField)
    {
    }

    public override void ValidateLinks(LinksValidationResult result)
    {
      Assert.ArgumentNotNull(result, "result");
      string value = base.Value;
      if (!string.IsNullOrEmpty(value))
      {
        HtmlDocument htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(value);
        this.AddTextLinks(result, htmlDocument);
        this.AddMediaLinks(result, htmlDocument);
      }
    }
    protected virtual void AddTextLinks(LinksValidationResult result, HtmlDocument document)
    {
      MethodInfo addTextLinksMethod = htmlFieldClassType.GetMethod("AddTextLinks", BindingFlags.Instance | BindingFlags.NonPublic);
      addTextLinksMethod.Invoke(this, new object[] { result, document });
    }

    protected virtual void AddMediaLinks(LinksValidationResult result, HtmlDocument document)
    {
      //covers img with other possible tags with src attribute, like audio, video, object/embeds etc.
      HtmlNodeCollection htmlNodeCollection = document.DocumentNode.SelectNodes("//*[@src]");
      if (htmlNodeCollection != null)
      {
        foreach (HtmlNode item in (IEnumerable<HtmlNode>)htmlNodeCollection)
        {
          addMediaLinkMethod.Invoke(this, new object[] { result, item });
        }
      }
    }
  }
}