export type Category = { id: number; name: string }

export type Subcategory = { id: number; categoryId: number; name: string }

export type Size = { id: number; categoryId: number; name: string }

export type Color = { id: number; name: string; hexCode: string }

export type Collection = { id: number; name: string }

export type Gender = { id: number; name: string }

export type ProductSize = { sizeId: number; stock: number }

export type Product = {
  id: number
  name: string
  price: number
  subcategoryId: number
  colorId: number
  collectionId: number | null
  genderId: number
  sizes: ProductSize[]
}

export type ProductSort = 'PriceAsc' | 'PriceDesc' | 'NameAsc' | 'NameDesc'

export type ProductFilter = {
  categoryId?: number
  subcategoryId?: number
  sizeId?: number
  colorId?: number
  collectionId?: number
  genderId?: number
  minPrice?: number
  maxPrice?: number
  sortBy?: ProductSort
}

export type Lookups = {
  categories: Category[]
  subcategories: Subcategory[]
  sizes: Size[]
  colors: Color[]
  collections: Collection[]
  genders: Gender[]
}
