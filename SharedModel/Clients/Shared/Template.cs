using System;
using Newtonsoft.Json;
using SharedModel.Helpers;

namespace SharedModel.Clients.Shared
{
	public class TemplateMessage 
	{
		public TemplateMessage(string to, Template template)
		{
			this.template = template;
			this.to = to;
		}

		public class Template
		{
			public Template(string name, List<Component> components, Language language = null)
			{
				this.Namespace = Settings.nameSpace;
				this.name = name;
				this.components = components;
				this.language = language ?? new Language();

			}

			public class Language
			{
				public Language(string code = "en")
				{
					this.code = code;
				}
				public string code { get; set; }
				public string policy { get; set; } = "deterministic";
			}

			public class Component
			{
				public Component(List<Parameter> parameters, string type = "body")
				{
					this.parameters = parameters;
					this.type = type;
				}
				public class Parameter
				{
					public Parameter(string text, string type = "text")
					{
						this.type = type;
						this.text = text;

					}
					public string type { get; set; }
					public string text { get; set; }
				}

				public string type { get; set; }
				public List<Parameter> parameters { get; set; }
			}
			
			[JsonProperty("namespace")]
			public string Namespace { get; set; }
			public string name { get; set; }
			public Language language { get; set; }
			public List<Component> components { get; set; }

		}
		public Template template { get; set; }
		public string to { get; set; }
		public string type { get; set; } = "template";

	}
}

