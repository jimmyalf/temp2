﻿using System;
using System.Collections.Generic;

namespace Spinit.Wpc.Synologen.Core.Domain.Model.Deviations
{
	public class DeviationComment : Entity
	{
        public virtual DateTime CreatedDate { get; set; }
        public virtual string Description { get; set; }
	}
}