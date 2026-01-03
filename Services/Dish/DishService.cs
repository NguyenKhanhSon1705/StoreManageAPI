using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Context;
using StoreManageAPI.DTO.Dish;
using StoreManageAPI.Helpers.Paging;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Dish;

namespace StoreManageAPI.Services.Dish
{
    public class DishService
        (
        ILogger<DishService> logger,
            DataStore context,
            IMapper mapper

        ) : IDishService
    {
        private readonly ILogger<DishService> logger = logger;
        private readonly IMapper mapper = mapper;
        private readonly DataStore context = context;

        public async Task<ApiResponse> AddPriceDishAsync(AddPriceDishDTO model)
        {
            try
            {
                var dish = await context.Dish.FindAsync(model.dish_id);
                if (dish == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy thông tin món ăn",
                        StatusCode = 404
                    };
                }
               
                //var checkExitsPrice = context.DishPriceVersions.Any(any => any.selling_price == model.new_price);
                var checkExitsPrice = context.DishPriceVersions.Any(dp => dp.dish_id == dish.Id && dp.selling_price == model.new_price);

                if (checkExitsPrice)
                {
                    return new ApiResponse
                    {
                        Message = "Giá này đã tồn tại trong món ăn",
                        StatusCode = 400
                    };
                }

                var newPrice = new DishPriceVersion
                {
                    dish_id = dish.Id,
                    selling_price = model.new_price,
                    create_at = DateTime.UtcNow,
                    status = false
                };
                context.DishPriceVersions.Add(newPrice);
                await context.SaveChangesAsync();
                var result = new PriceDishInfoDTO
                {
                    dish_id = newPrice.dish_id,
                    create_at = newPrice.create_at,
                    price_id = newPrice.id,
                    selling_price  = newPrice.selling_price,
                    status = newPrice.status
                };
                return new ApiResponse 
                { 
                    Data = result,
                    Message = "Thêm giá mới cho món ăn thành công",
                    IsSuccess = true,
                    StatusCode = 200
                };
            }
            catch ( Exception ex )
            {
                logger.LogError($"There are something error in DishService/AddPriceDishAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in DishService/AddPriceDishAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> CreateDishAsync(CreateDishV model)
        {
            try
            {
                var checkDishName = await context.Menu_Groups_Dish
                    .Where(w => w.Menu_Group.ShopId == model.Shop_Id)
                    .AnyAsync(a => a.Dish.Dish_Name == model.Dish_Name);

                if (checkDishName)
                {
                    return new ApiResponse
                    {
                        Message = "Tên món ăn đã tồn tại",
                        StatusCode = 409
                    };
                }

                var newDish = mapper.Map<Models.Dish>(model);
                newDish.Create_At = DateTime.UtcNow;

                var sellingPrice = new DishPriceVersion
                {
                    price_version = "version 1",
                    selling_price = model.Selling_Price,
                    status = true,
                    create_at = DateTime.UtcNow,
                    dish = newDish
                };
                context.DishPriceVersions.Add(sellingPrice);
                context.Dish.Add(newDish);

                IList<ObMenuGroupV> menuGroup = new List<ObMenuGroupV>();
                if(model.Arr_Menu_Group_Id != null)
                {
                    foreach(int id in model.Arr_Menu_Group_Id)
                    {
                        var between = new MenuGroupDish
                        {
                            Menu_Group_Id = id,
                            Dish = newDish
                        };
                        var menu = context.MenuGroups.Find(id);
                        menuGroup.Add(new ObMenuGroupV { Id = menu.Id , Name = menu.Name});
                        context.Menu_Groups_Dish.Add(between);
                    }
                }
                await context.SaveChangesAsync();
                
                var result = mapper.Map<DishV>(newDish);
                result.Arr_Menu_Group = menuGroup;
                result.Shop_Id = model.Shop_Id;
                result.list_price = new List<DTO.Dish.PriceDishInfoDTO>
                {
                    new DTO.Dish.PriceDishInfoDTO
                    {
                        create_at = sellingPrice.create_at,
                        selling_price = sellingPrice.selling_price,
                        status = sellingPrice.status,
                        price_id = sellingPrice.id
                    }
                };
                
                return new ApiResponse
                {
                    Message = "Tạo món ăn thành công",
                    StatusCode = 201,
                    Data = result,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in DishService/CreateDishAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in DishService/CreateDishAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> DeleteDishAsync(int Id)
        {
            try
            {
                var dish = await context.Dish.FindAsync(Id);
                if (dish == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy thông tin món ăn",
                        StatusCode = 404
                    };
                }
                context.Dish.Remove(dish);
                await context.SaveChangesAsync();

                return new ApiResponse
                {
                    Message = "Xóa món ăn thành công",
                    StatusCode = 200,
                    Data = dish,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in DishService/DeleteDishAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in DishService/DeleteDishAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> DeletePriceDishAsync(int id)
        {
            try
            {
                var exitsPrice = await context.DishPriceVersions.FindAsync(id);
                if(exitsPrice == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy thông tin giá hiện tại",
                        StatusCode = 404
                    };
                }
                if (exitsPrice.status == true)
                {
                    return new ApiResponse
                    {
                        Message = "Giá này đang được sử dụng không thể xóa",
                        StatusCode = 400,
                    };
                }

                context.DishPriceVersions.Remove(exitsPrice);
                await context.SaveChangesAsync();
                return new ApiResponse
                {
                    Message = $"Xóa giá {exitsPrice.selling_price} thành công",
                    StatusCode = 200,
                    IsSuccess=true,
                    Data = exitsPrice
                };
            }
            catch (Exception ex )
            {
                logger.LogError($"There are something error in DishService/DeletePriceDishAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in DishService/DeletePriceDishAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetAllDishAsync(int shopId, int pageIndex ,int pageSize, string search )
        {
            try
            {
                var dishMenu = context.Menu_Groups_Dish
                    .Include(i => i.Dish) 
                        .ThenInclude(d => d.Price) 
                    .Include(i => i.Menu_Group)
                    .AsQueryable();

                dishMenu = dishMenu.Where(w => w.Menu_Group.ShopId == shopId &&
                                (string.IsNullOrWhiteSpace(search) || w.Dish.Dish_Name.Contains(search)));

                var listMenuDish = dishMenu.GroupBy(x => new
                {
                    x.Dish.Dish_Name,
                    x.Dish.Id,
                    x.Dish.Image,
                    x.Dish.Unit_Name,
                    x.Dish.Origin_Price,
                    
                    x.Dish.Order,
                    x.Dish.Status,
                    x.Dish.Is_Hot

                }).Select(group => new DishV
                {
                    Dish_Name = group.Key.Dish_Name,
                    Id = group.Key.Id,
                    Image = group.Key.Image,
                    Unit_Name = group.Key.Unit_Name,
                    Origin_Price = group.Key.Origin_Price,
                    Order = group.Key.Order,
                    Status = group.Key.Status,
                    Is_Hot = group.Key.Is_Hot ,

                    list_price = group.Select(x=> new DTO.Dish.PriceDishInfoDTO
                    {
                        price_id = x.Dish.Price.id,
                        selling_price = x.Dish.Price.selling_price,
                        status = x.Dish.Price.status,
                        create_at = x.Dish.Price.create_at
                    }).Distinct().ToList(),

                    Arr_Menu_Group = group.Select(x => new ObMenuGroupV
                    {
                        Id = x.Menu_Group.Id,
                        Name = x.Menu_Group.Name
                    }).ToList()
                });

                var result = await Paging<DishV, DishV>.CreateAsync(listMenuDish, pageIndex, pageSize);
                var list = new Paging<DishV, DishV>(mapper.Map<List<DishV>>(result.Items), result.TotalCount, result.PageIndex, result.PageSize);

                return new ApiResponse
                {
                    Message = "Danh sách món ăn",
                    StatusCode = 200,
                    Data = list,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in DishService/GetAllDishAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in DishService/GetAllDishAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetAllDishByGroupMenu(string? search, int pageIndex = 1, int pageSize = 10, int? menuGroupId = null, int? shopId = null)
        {
            try
            {

                var query = context.Menu_Groups_Dish
                            .Include(w => w.Menu_Group)
                            .Include(w => w.Dish)
                            .ThenInclude(t => t.Price)
                            .Where(w => w.Dish.Price.status == true)
                            .Where(w => w.Menu_Group.ShopId == shopId);

                if (menuGroupId != null)
                {
                    query = query.Where(w => w.Menu_Group_Id == menuGroupId);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(w => w.Dish.Dish_Name.Contains(search));
                }

                var kq = query
                    .GroupBy(s => new
                    {
                        s.Dish.Id,
                        s.Dish.Dish_Name,
                        s.Dish.Inventory,
                        s.Dish.Image,
                        s.Dish.Unit_Name,
                        s.Dish.Price.selling_price,
                        s.Dish.Price.id
                    })
                    .Select(g => new DishInfo
                    {
                        Id = g.Key.Id,
                        Dish_Name = g.Key.Dish_Name,
                        Inventory = g.Key.Inventory,
                        Image = g.Key.Image,
                        Unit_Name = g.Key.Unit_Name,
                        Selling_Price = g.Key.selling_price,
                        selling_price_id = g.Key.id
                    });


                var result = await Paging<DishInfo, DishInfo>.CreateAsync(kq, pageIndex, pageSize);
                var list = new Paging<DishInfo, DishInfo>(
                    mapper.Map<List<DishInfo>>(result.Items),
                    result.TotalCount,
                    result.PageIndex,
                    result.PageSize
                );

                return new ApiResponse
                {
                    Message = "Danh sách món ăn",
                    StatusCode = 200,
                    Data = list,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in DishService/GetAllDishByGroupMenu: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in DishService/GetAllDishByGroupMenu: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> UpdateDishAsync(CreateDishV model)
        {
           
            try
            {
                var checkDishName = await context.Menu_Groups_Dish
                                    .Where(w => w.Menu_Group.ShopId == model.Shop_Id && w.Dish.Dish_Name == model.Dish_Name && w.Dish.Id != model.Id)
                                    .AnyAsync();

                if (checkDishName)
                {
                    return new ApiResponse
                    {
                        Message = "Tên món ăn đã tồn tại",
                        StatusCode = 409
                    };
                }

                // Tìm đối tượng `Dish` cần cập nhật
                var dish = await context.Dish.FindAsync(model.Id);

                if (dish == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy thông tin món ăn",
                        StatusCode = 404
                    };
                }

                var resultPrice = new List<PriceDishInfoDTO>();
                var listPrice = await context.DishPriceVersions.Where(w => w.dish_id == model.Id).ToListAsync();

                if (model.Selling_Price != null)
                {
                   
                    foreach (var price in listPrice)
                    {
                        var existingDishId = price.dish_id; 
                        context.DishPriceVersions.Attach(price);

                        price.status = price.selling_price == model.Selling_Price;

                        // Gán lại giá trị dish_id
                        price.dish_id = existingDishId;

                        context.Entry(price).Property(p => p.status).IsModified = true;
                        context.Entry(price).Property(p => p.dish_id).IsModified = false;
                    }

                    resultPrice = listPrice.Select(price => new PriceDishInfoDTO
                    {
                        dish_id = price.dish_id,
                        price_id = price.id,
                        status = price.status,
                        selling_price = price.selling_price,
                        create_at = price.create_at
                    }).ToList();
                }
                else
                {
                    resultPrice = mapper.Map<List<PriceDishInfoDTO>>(listPrice);

                }

                // Cập nhật thông tin của `dish`
                dish.Dish_Name = model.Dish_Name ?? dish.Dish_Name;
                dish.Unit_Name = model.Unit_Name ?? dish.Unit_Name;
                dish.Origin_Price = model.Origin_Price ?? dish.Origin_Price;
                dish.Order = model.Order ?? dish.Order;
                dish.Is_Hot = model.Is_Hot ?? dish.Is_Hot;
                dish.Status = model.Status ?? dish.Status;
                dish.Image = model.Image ?? dish.Image;



                // Thêm các nhóm menu mới từ `model.Arr_Menu_Group_Id`
                IList<ObMenuGroupV> menuGroup = new List<ObMenuGroupV>();
                if (model.Arr_Menu_Group_Id.Count > 0)
                {
                    var existingMenuGroupDishes = context.Menu_Groups_Dish
                    .Where(mgd => mgd.Dish.Id == dish.Id)
                    .ToList();

                    context.Menu_Groups_Dish.RemoveRange(existingMenuGroupDishes);

                    foreach (int id in model.Arr_Menu_Group_Id)
                    {
                        var between = new MenuGroupDish
                        {
                            Menu_Group_Id = id,
                            Dish = dish
                        };
                       
                        var menu = await context.MenuGroups.FindAsync(id);
                        if (menu != null)
                        {
                            menuGroup.Add(new ObMenuGroupV { Id = menu.Id, Name = menu.Name });
                            context.Menu_Groups_Dish.Add(between); 
                        }
                    }
                }
                else
                {
                    var existingMenuGroupDishes = await context.Menu_Groups_Dish
                               .Include(inc=> inc.Menu_Group)
                               .Where(mgd => mgd.Dish.Id == dish.Id)
                               .ToListAsync();
                    existingMenuGroupDishes.ForEach(item => menuGroup.Add( new ObMenuGroupV { Id = item.Menu_Group.Id , Name = item.Menu_Group.Name }));
                }

                var result = mapper.Map<DishV>(dish);
                result.Arr_Menu_Group = menuGroup;
                result.list_price = resultPrice;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await context.SaveChangesAsync();

                return new ApiResponse
                {
                    Message = "Cập nhật nhóm món ăn thành công",
                    StatusCode = 200,
                    Data = result,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in DishService/UpdateDishAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in DishService/UpdateDishAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
    
    
    }
}
