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
    
    public partial class tb_Norms
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_Norms()
        {
            this.tb_Prodect = new HashSet<tb_Prodect>();
        }
    
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    
        public virtual tb_Language tb_Language { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_Prodect> tb_Prodect { get; set; }
    }
}
