#define RENDERCACHE

//
// ViewEmployee.ascx.cache.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Web.UI;
using DotNetNuke.Common.Utilities;
using R7.University;

namespace R7.University.EmployeeList
{
	public partial class ViewEmployeeList
	{
		#if (RENDERCACHE)
		
		private bool renderDisabled = false;
		private string renderCacheContent;

		private string DataCacheKey
		{
			get { return "EmployeeList_" + TabModuleId; }
		}

		protected override void Render (HtmlTextWriter writer)
		{
			if (!renderDisabled)
			{
				if (renderCacheContent == null)
				{
					// cache expired
					using (var stringWriter = new System.IO.StringWriter ())
					using (var htmlWriter = new HtmlTextWriter (stringWriter))
					{
						// render the current page content to our temp writer
						base.Render (htmlWriter);
						// htmlWriter.Flush();
						htmlWriter.Close ();

						// get the content
						renderCacheContent = stringWriter.ToString ();
					}
		
					// set render cache only in non-edit mode - 
                    // this gives editors somewhat increased performance and ensures that common users see only allowed content
					if (!IsEditable)
					{
						var settings = new EmployeeListSettings (this);
						CacheHelper.Set<string> (renderCacheContent, DataCacheKey, settings.DataCacheTime);
					}
				}

				// write the new html to the page
				writer.Write (renderCacheContent);
			}
		}
		
		/*
		protected void ClearDataCache_Action (object sender, ActionEventArgs e)
		{
			try 
			{
				if (e.Action.CommandName == "ClearDataCache.Action")
				{
					DataCache.RemoveCache(DataCacheKey);

					// Disabling render as we don't have controls, filled with data.
					disableRender = true; 

					// NOTE: Also, control binding could be done in LoadComplete,
					// so we should know that we need update view, and there
					// be no need to disable render

					Response.Redirect (Globals.NavigateURL (), true);
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}*/
		
		#endif
		
		/// <summary>
		/// Apply necessary cache logic on load
		/// </summary>
		/// <returns><c>true</c>, if control must render from cache, <c>false</c> otherwise.</returns>
		private bool Cache_OnLoad ()
		{
			var cacheExists = false;
		
			#if (RENDERCACHE)
			
			var settings = new EmployeeListSettings (this);
			
			if (settings.DataCacheTime > 0)
			{
				if (IsEditable)
				{
					// don't use cache in edit mode
					DataCache.RemoveCache (DataCacheKey);
					// DataCache.RemoveCache(dataCacheKey + "_ContainerVisible");
				}
				else
				{
					// read cache here, cause it may be too late on render
					renderCacheContent = CacheHelper.Get<string> (DataCacheKey); 
				
					if (renderCacheContent != null)
					{
						// REVIEW: Maybe is would be better is set default container visibility to true?
						var visible = CacheHelper.TryGet<bool> (DataCacheKey + "_ContainerVisible", false);
						
						ContainerControl.Visible = visible;
						renderDisabled = !visible;
						
						// no need to do anything, just render from cache
						cacheExists = true;
					}
				}
			}
			
			#endif
			
			return cacheExists;
		}

		
		/// <summary>
		/// Stores container control visibility to data cache
		/// </summary>
		/// <param name="visible">Set to <c>true</c>, if container control should be visible to common users.</param>
		private void Cache_SetContainerVisible (bool visible)
		{
			#if (RENDERCACHE)
		
			var settings = new EmployeeListSettings (this);
			CacheHelper.Set<bool> (visible, DataCacheKey + "_ContainerVisible", settings.DataCacheTime + 60);
			
			#endif
		}
		
	}
}
