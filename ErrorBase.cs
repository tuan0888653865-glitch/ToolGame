using System;

namespace TinhKiemAuto
{
	// Token: 0x0200008F RID: 143
	[Serializable]
	public abstract class ErrorBase
	{
		// Token: 0x0600065F RID: 1631 RVA: 0x000246E1 File Offset: 0x000228E1
		public virtual void ClearErrors()
		{
			this._lasterror = null;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x000246EA File Offset: 0x000228EA
		public virtual Exception GetLastError()
		{
			return this._lasterror;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000246F2 File Offset: 0x000228F2
		protected virtual bool SetLastError(Exception e)
		{
			this._lasterror = e;
			return false;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000246FC File Offset: 0x000228FC
		protected virtual bool SetLastError(string message)
		{
			return this.SetLastError(new Exception(message));
		}

		// Token: 0x04000423 RID: 1059
		protected Exception _lasterror;
	}
}
