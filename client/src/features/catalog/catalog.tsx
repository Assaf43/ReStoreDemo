import agent from "../../app/api/agent";
import LoadingComponenet from "../../app/layout/LoadingComponent";
import { Product } from "../../app/models/product";
import ProductList from "./productList";
import { useState, useEffect } from "react";

export default function Catalog() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    agent.Catalog.list()
      .then((products) => setProducts(products))
      .catch((error) => console.log(error))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <LoadingComponenet message="Loading Products..." />;

  return (
    <>
      <ProductList products={products} />
    </>
  );
}
