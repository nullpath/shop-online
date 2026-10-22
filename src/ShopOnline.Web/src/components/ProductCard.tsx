import type { Collection, Color, Gender, Product, Size, Subcategory } from '../api/types'

type ProductCardProps = {
  product: Product
  subcategory?: Subcategory
  color?: Color
  gender?: Gender
  collection?: Collection
  sizes: Map<number, Size>
}

const priceFormat = new Intl.NumberFormat('pl-PL', { style: 'currency', currency: 'PLN' })

export function ProductCard({ product, subcategory, color, gender, collection, sizes }: ProductCardProps) {
  return (
    <article className="card">
      <div className="card-image" style={{ backgroundColor: color?.hexCode }}>
        <span className="card-type">{subcategory?.name}</span>
        {collection && <span className="card-badge">{collection.name}</span>}
      </div>

      <div className="card-body">
        <p className="card-meta">
          {gender?.name}
          {color && ` · ${color.name}`}
        </p>
        <h3 className="card-title">{product.name}</h3>
        <p className="card-price">{priceFormat.format(product.price)}</p>

        <ul className="card-sizes">
          {product.sizes.map((productSize) => (
            <li key={productSize.sizeId} className={productSize.stock > 0 ? '' : 'sold-out'}>
              {sizes.get(productSize.sizeId)?.name}
            </li>
          ))}
        </ul>
      </div>
    </article>
  )
}
