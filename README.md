# ShopOnline

Simple clothing shop I built while learning ASP.NET Core. One page with products (t-shirts, pants, gloves) and filters for category, type, size, color, collection, gender and price.

Backend: ASP.NET Core 10 minimal API, EF Core, SQLite
Frontend: React + TypeScript, Vite

## How to run

Requires .NET 10 SDK and Node.js 22+.

```bash
git clone <repo-url>
cd shop-online

# API
dotnet run --project src/ShopOnline.Api

# frontend (second terminal)
cd src/ShopOnline.Web
npm install
npm run dev
```

Then open http://localhost:5173

Database is created on first run and seeded with sample data.

## Structure

```
src/
  ShopOnline.Domain           entities
  ShopOnline.Application      DTOs, mapping, service interfaces
  ShopOnline.Infrastructure   DbContext, migrations, seed, services
  ShopOnline.Api              endpoints
  ShopOnline.Web              React app
```

## API

CRUD for `/products`, `/categories`, `/subcategories`, `/sizes`, `/colors`, `/collections`, `/genders`.

Products can be filtered and sorted:

```
GET /products?categoryId=3&genderId=2&minPrice=50&maxPrice=200&sortBy=PriceDesc
```

Other params: `subcategoryId`, `sizeId`, `colorId`, `collectionId`. Sort options: `PriceAsc`, `PriceDesc`, `NameAsc`, `NameDesc`.

More examples in `src/ShopOnline.Api/ShopOnline.Api.http`.
