//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BowntDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_File
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TypeId { get; set; }
        public string Path { get; set; }
    
        public virtual tb_FileType tb_FileType { get; set; }
    }
}
