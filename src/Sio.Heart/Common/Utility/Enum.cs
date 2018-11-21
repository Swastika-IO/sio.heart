// Licensed to the Siocore Foundation under one or more agreements.
// The Siocore Foundation licenses this file to you under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

namespace Sio.Common.Utility
{
    public class Enums
    {
        public enum SWStatus
        {
            Deleted = 0,
            Preview = 1,
            Published = 2,
            Draft = 3,
            Schedule = 4
        }

        public enum ResponseKey
        {
            NotFound,
            OK,
            BadRequest
        }
    }
}
