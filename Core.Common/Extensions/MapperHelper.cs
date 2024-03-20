using AutoMapper;

namespace Core.Common.Extensions {
    public static class MapperHelper {
        public static IList<TDest> ToViewModelList<TSource, TDest>(this IList<TSource> cust) {
            IList<TDest> list = new List<TDest>();
            if (!(cust != null && cust.Any())) {
                return list;
            }

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDest>();
            });

            var mapper = config.CreateMapper();
            foreach (var item in cust) {
                list.Add(mapper.Map<TSource, TDest>(item));
            }

            return list;
        }

        public static TDest ToViewModel<TSource, TDest>(this TSource cust) {

            if (cust == null) {
                return default(TDest);
            }
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDest>();
            });

            var mapper = config.CreateMapper();
            TDest dest = mapper.Map<TSource, TDest>(cust);
            return dest;

        }
    }
}
