﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.NewsDtos
{
	public class NewsDto
	{
		public int NewsId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string img { get; set; }
		public DateTime News_Date { get; set; }
	}
}