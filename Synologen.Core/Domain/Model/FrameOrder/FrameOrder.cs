using System;

namespace Spinit.Wpc.Synologen.Core.Domain.Model.FrameOrder
{
	public class FrameOrder
	{
		public virtual int Id { get; set; }
		public virtual Frame Frame { get; set; }
		public virtual FrameGlassType GlassType { get; set; }
		public virtual Shop OrderingShop { get; set; }
		public virtual EyeParameter PupillaryDistance { get; set; }
		public virtual EyeParameter Sphere { get; set; }
		public virtual EyeParameter Cylinder { get; set; }
		public virtual EyeParameter Axis { get; set; }
		public virtual EyeParameter Addition { get; set; }
		public virtual EyeParameter Height { get; set; }
		public virtual DateTime Created { get; set; }
		public virtual DateTime? Sent { get; set; }
		public virtual string Notes { get; set; }
		public virtual bool IsSent
		{
			get { return (Sent.HasValue); }
		}

		
	}
}