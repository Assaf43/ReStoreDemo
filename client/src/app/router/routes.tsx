import { Navigate, createBrowserRouter } from "react-router-dom";
import App from "../layout/App";
import HomePage from "../../features/home/home";
import Catalog from "../../features/catalog/catalog";
import ProductDetails from "../../features/catalog/productDetails";
import AboutPage from "../../features/about/aboutPage";
import ContactPage from "../../features/contact/contactPage";
import ServerError from "../error/ServerError";
import NotFound from "../error/NotFound";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "", element: <HomePage /> },
      { path: "catalog", element: <Catalog /> },
      { path: "catalog/:id", element: <ProductDetails /> },
      { path: "about", element: <AboutPage /> },
      { path: "contact", element: <ContactPage /> },
      { path: "server-error", element: <ServerError /> },
      { path: "not-found", element: <NotFound /> },
      { path: "*", element: <Navigate replace to={"/not-found"} /> },
    ],
  },
]);
