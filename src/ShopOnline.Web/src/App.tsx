import { useEffect, useMemo, useState } from 'react'
import { getLookups, getProducts } from './api/client'
import type { Lookups, Product, ProductFilter } from './api/types'
import { FilterPanel } from './components/FilterPanel'
import { ProductCard } from './components/ProductCard'
import './App.css'

function byId<T extends { id: number }>(items: T[]): Map<number, T> {
  return new Map(items.map((item) => [item.id, item]))
}

export default function App() {
  const [lookups, setLookups] = useState<Lookups | null>(null)
  const [filter, setFilter] = useState<ProductFilter>({})
  const [products, setProducts] = useState<Product[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    getLookups()
      .then(setLookups)
      .catch(() => setError('Could not load filters. Is the API running?'))
  }, [])

  useEffect(() => {
    const controller = new AbortController()

    const timeout = setTimeout(() => {
      setLoading(true)

      getProducts(filter, controller.signal)
        .then((result) => {
          setProducts(result)
          setError(null)
        })
        .catch(() => {
          if (!controller.signal.aborted) setError('Could not load products. Is the API running?')
        })
        .finally(() => {
          if (!controller.signal.aborted) setLoading(false)
        })
    }, 250)

    return () => {
      clearTimeout(timeout)
      controller.abort()
    }
  }, [filter])

  const maps = useMemo(
    () =>
      lookups && {
        subcategories: byId(lookups.subcategories),
        sizes: byId(lookups.sizes),
        colors: byId(lookups.colors),
        genders: byId(lookups.genders),
        collections: byId(lookups.collections),
      },
    [lookups],
  )

  return (
    <div className="page">
      <header className="header">
        <h1>ShopOnline</h1>
        <p>Clothes and accessories for women and men</p>
      </header>

      <main className="layout">
        {lookups && <FilterPanel lookups={lookups} filter={filter} onChange={setFilter} />}

        <section className="results">
          <p className="results-count">
            {loading ? 'Loading...' : `${products.length} ${products.length === 1 ? 'product' : 'products'}`}
          </p>

          {error && <p className="message error">{error}</p>}

          {!loading && !error && products.length === 0 && (
            <p className="message">No products match these filters.</p>
          )}

          {maps && (
            <div className="grid">
              {products.map((product) => (
                <ProductCard
                  key={product.id}
                  product={product}
                  subcategory={maps.subcategories.get(product.subcategoryId)}
                  color={maps.colors.get(product.colorId)}
                  gender={maps.genders.get(product.genderId)}
                  collection={product.collectionId ? maps.collections.get(product.collectionId) : undefined}
                  sizes={maps.sizes}
                />
              ))}
            </div>
          )}
        </section>
      </main>
    </div>
  )
}
