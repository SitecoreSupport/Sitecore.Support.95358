using HtmlAgilityPack;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sitecore.Support.Data.Fields
{
  public class HtmlField : Sitecore.Data.Fields.HtmlField
  {
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
        this.AddLinksMediaFlashManagers(result, htmlDocument);
        this.AddVideoLink(result, htmlDocument);
      }
    }
    protected virtual void AddTextLinks(LinksValidationResult result, HtmlDocument document)
    {
      Type typeFromHandle = typeof(Sitecore.Data.Fields.HtmlField);
      MethodInfo method = typeFromHandle.GetMethod("AddTextLinks", BindingFlags.Instance | BindingFlags.NonPublic);
      object obj = method.Invoke(this, new object[2]
      {
                result,
                document
      });
    }

    protected virtual void AddMediaLinks(LinksValidationResult result, HtmlDocument document)
    {
      Type typeFromHandle = typeof(Sitecore.Data.Fields.HtmlField);
      MethodInfo method = typeFromHandle.GetMethod("AddMediaLinks", BindingFlags.Instance | BindingFlags.NonPublic);
      object obj = method.Invoke(this, new object[2]
      {
            result,
            document
      });
    }

    protected virtual void AddLinksMediaFlashManagers(LinksValidationResult result, HtmlDocument document)
    {
      Assert.ArgumentNotNull(result, "result");
      Assert.ArgumentNotNull(document, "document");
      HtmlNodeCollection htmlNodeCollection = document.DocumentNode.SelectNodes("//object");
      if (htmlNodeCollection != null)
      {
        IEnumerable<HtmlNode> enumerable = from n in htmlNodeCollection.Nodes()
                                           where n.Name == "embed"
                                           select n;
        foreach (HtmlNode item in enumerable)
        {
          this.AddLinkMediaManager1(result, item);
        }
      }
    }

    protected virtual void AddVideoLink(LinksValidationResult result, HtmlDocument document)
    {
      Assert.ArgumentNotNull(result, "result");
      Assert.ArgumentNotNull(document, "document");
      HtmlNodeCollection htmlNodeCollection = document.DocumentNode.SelectNodes("//video");
      if (htmlNodeCollection != null)
      {
        foreach (HtmlNode item in (IEnumerable<HtmlNode>)htmlNodeCollection)
        {
          this.AddLinkMediaManager1(result, item);
        }
      }
    }

    protected virtual void AddLinkMediaManager1(LinksValidationResult result, HtmlNode node)
    {
      Type typeFromHandle = typeof(Sitecore.Data.Fields.HtmlField);
      MethodInfo method = typeFromHandle.GetMethod("AddMediaLink", BindingFlags.Instance | BindingFlags.NonPublic);
      object obj = method.Invoke(this, new object[2]
      {
            result,
            node
      });
    }
  }
}