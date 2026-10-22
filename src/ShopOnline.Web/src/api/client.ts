import type {
  Category,
  Collection,
  Color,
  Gender,
  Lookups,
  Product,
  ProductFilter,
  Size,
  Subcategory,
} from './types'

const API_URL = '/api'

async function get<T>(path: string, signal?: AbortSignal): Promise<T> {
  const response = await fetch(`${API_URL}${path}`, { signal })

  if (!response.ok) {
    throw new Error(`GET ${path} failed with ${response.status}`)
  }

  return (await response.json()) as T
}

export async function getLookups(): Promise<Lookups> {
  const [categories, subcategories, sizes, colors, collections, genders] = await Promise.all([
    get<Category[]>('/categories'),
    get<Subcategory[]>('/subcategories'),
    get<Size[]>('/sizes'),
    get<Color[]>('/colors'),
    get<Collection[]>('/collections'),
    get<Gender[]>('/genders'),
  ])

  return { categories, subcategories, sizes, colors, collections, genders }
}

export function getProducts(filter: ProductFilter, signal?: AbortSignal): Promise<Product[]> {
  const params = new URLSearchParams()

  for (const [key, value] of Object.entries(filter)) {
    if (value !== undefined) {
      params.set(key, String(value))
    }
  }

  const query = params.toString()

  return get<Product[]>(query ? `/products?${query}` : '/products', signal)
}
