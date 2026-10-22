import type { Lookups, ProductFilter, ProductSort } from '../api/types'

type FilterPanelProps = {
  lookups: Lookups
  filter: ProductFilter
  onChange: (filter: ProductFilter) => void
}

const sortOptions: { value: ProductSort; label: string }[] = [
  { value: 'PriceAsc', label: 'Price: low to high' },
  { value: 'PriceDesc', label: 'Price: high to low' },
  { value: 'NameAsc', label: 'Name: A-Z' },
  { value: 'NameDesc', label: 'Name: Z-A' },
]

function toNumber(value: string): number | undefined {
  return value === '' ? undefined : Number(value)
}

export function FilterPanel({ lookups, filter, onChange }: FilterPanelProps) {
  const subcategories = lookups.subcategories.filter((s) => s.categoryId === filter.categoryId)
  const sizes = lookups.sizes.filter((s) => s.categoryId === filter.categoryId)

  const update = (changes: ProductFilter) => onChange({ ...filter, ...changes })

  const toggle = (key: 'genderId' | 'colorId' | 'sizeId', id: number) =>
    update({ [key]: filter[key] === id ? undefined : id })

  return (
    <aside className="filters">
      <div className="filters-header">
        <h2>Filters</h2>
        <button type="button" className="link-button" onClick={() => onChange({ sortBy: filter.sortBy })}>
          Clear all
        </button>
      </div>

      <label className="field">
        <span>Sort by</span>
        <select
          value={filter.sortBy ?? ''}
          onChange={(e) => update({ sortBy: (e.target.value || undefined) as ProductSort | undefined })}
        >
          <option value="">Newest</option>
          {sortOptions.map((option) => (
            <option key={option.value} value={option.value}>
              {option.label}
            </option>
          ))}
        </select>
      </label>

      <fieldset className="field">
        <legend>Gender</legend>
        <div className="chips">
          {lookups.genders.map((gender) => (
            <button
              key={gender.id}
              type="button"
              className={filter.genderId === gender.id ? 'chip active' : 'chip'}
              onClick={() => toggle('genderId', gender.id)}
            >
              {gender.name}
            </button>
          ))}
        </div>
      </fieldset>

      <label className="field">
        <span>Category</span>
        <select
          value={filter.categoryId ?? ''}
          onChange={(e) =>
            update({ categoryId: toNumber(e.target.value), subcategoryId: undefined, sizeId: undefined })
          }
        >
          <option value="">All categories</option>
          {lookups.categories.map((category) => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>
      </label>

      {filter.categoryId !== undefined && (
        <>
          <label className="field">
            <span>Type</span>
            <select
              value={filter.subcategoryId ?? ''}
              onChange={(e) => update({ subcategoryId: toNumber(e.target.value) })}
            >
              <option value="">All types</option>
              {subcategories.map((subcategory) => (
                <option key={subcategory.id} value={subcategory.id}>
                  {subcategory.name}
                </option>
              ))}
            </select>
          </label>

          <fieldset className="field">
            <legend>Size</legend>
            <div className="chips">
              {sizes.map((size) => (
                <button
                  key={size.id}
                  type="button"
                  className={filter.sizeId === size.id ? 'chip active' : 'chip'}
                  onClick={() => toggle('sizeId', size.id)}
                >
                  {size.name}
                </button>
              ))}
            </div>
          </fieldset>
        </>
      )}

      <fieldset className="field">
        <legend>Color</legend>
        <div className="swatches">
          {lookups.colors.map((color) => (
            <button
              key={color.id}
              type="button"
              title={color.name}
              aria-label={color.name}
              className={filter.colorId === color.id ? 'swatch active' : 'swatch'}
              style={{ backgroundColor: color.hexCode }}
              onClick={() => toggle('colorId', color.id)}
            />
          ))}
        </div>
      </fieldset>

      <label className="field">
        <span>Collection</span>
        <select
          value={filter.collectionId ?? ''}
          onChange={(e) => update({ collectionId: toNumber(e.target.value) })}
        >
          <option value="">All collections</option>
          {lookups.collections.map((collection) => (
            <option key={collection.id} value={collection.id}>
              {collection.name}
            </option>
          ))}
        </select>
      </label>

      <fieldset className="field">
        <legend>Price</legend>
        <div className="price-range">
          <input
            type="number"
            min={0}
            placeholder="Min"
            value={filter.minPrice ?? ''}
            onChange={(e) => update({ minPrice: toNumber(e.target.value) })}
          />
          <span>-</span>
          <input
            type="number"
            min={0}
            placeholder="Max"
            value={filter.maxPrice ?? ''}
            onChange={(e) => update({ maxPrice: toNumber(e.target.value) })}
          />
        </div>
      </fieldset>
    </aside>
  )
}
