using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Cardio.UI.Persistence
{
	public interface IPersistent
	{
		XElement SaveState();

		void LoadState(XElement element);
	}
}
