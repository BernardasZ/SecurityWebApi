using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mappings
{
	public class BaseMapping<TModel> : IEntityTypeConfiguration<TModel> where TModel : BaseModel
	{
		public void Configure(EntityTypeBuilder<TModel> builder)
		{
		}
	}
}