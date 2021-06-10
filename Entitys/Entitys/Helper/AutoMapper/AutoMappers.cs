using AutoMapper;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation;
using Entitys.ViewModels.CashOperation.Book110;
using Entitys.ViewModels.CashOperation.Book141AViewModel;
using Entitys.ViewModels.CashOperation.Book141ViewModel;
using Entitys.ViewModels.CashOperation.Book155;
using Entitys.ViewModels.CashOperation.Journal16VM;
using Entitys.ViewModels.CashOperation.Journal176ViewModel;

namespace Entitys.Helper.AutoMapper
{
    public class AutoMappers:Profile
    {
        public AutoMappers()
        {
            #region Book175s
            CreateMap<Book155, Book175ViewModel>();
            CreateMap<Book175ViewModel, Book155>();

            CreateMap<Book155, Book175PostViewModel>();
            CreateMap<Book175PostViewModel, Book155>();

            CreateMap<Book155, Book175PutViewModel>();
            CreateMap<Book175PutViewModel, Book155>();
            #endregion            

            #region Book16
            CreateMap<Journal16, Journal16ViewModel>();
            CreateMap<Journal16ViewModel, Journal15>();

            CreateMap<Journal16, Journal16PutViewModel>();
            CreateMap<Journal16PutViewModel, Journal16>();

            CreateMap<Journal16, Journal16PostViewModel>();
            CreateMap<Journal16PostViewModel, Journal16>();
            #endregion

            #region Book110
            CreateMap<Journal110, Book110ViewModel>();
            CreateMap<Book110ViewModel, Journal110>();

            CreateMap<Journal110, Book110PutViewModel>();
            CreateMap<Book110PutViewModel, Journal110>();

            CreateMap<Journal110, Book110PostViewModel>();
            CreateMap<Book110PostViewModel, Journal110>();
            #endregion

            #region Book141
            CreateMap<Book141, Book141ViewModel>();
            CreateMap<Book141ViewModel, Book141>();

            CreateMap<Book141, Book141PutViewModel>();
            CreateMap<Book141PutViewModel, Book141>();

            CreateMap<Book141, Book141PostViewModel>();
            CreateMap<Book141PostViewModel, Book141>();
            #endregion

            #region Book141A
            CreateMap<Book141A, Book141AViewModel>();
            CreateMap<Book141AViewModel, Book141A>();

            CreateMap<Book141A, Book141APutViewModel>();
            CreateMap<Book141APutViewModel, Book141A>();

            CreateMap<Book141, Book141APostViewModel>();
            CreateMap<Book141APostViewModel, Book141A>();
            #endregion

            #region Book155
            CreateMap<Book155, Book155PutViewModel>();
            CreateMap<Book155PutViewModel, Book155>();
            #endregion

            #region
            CreateMap<Journal176, Journal176PostViewModel>();
            CreateMap<Journal176PostViewModel, Journal176>();

            CreateMap<Journal176, Journal176PutViewModel>();
            CreateMap<Journal176PutViewModel, Journal176>();
            #endregion

        }
    }
}
