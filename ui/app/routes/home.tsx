import HomePage from "~/components/HomePage/HomePage";
import type { Route } from "./+types/home";
import MainLayout from "~/layouts/MainLayout";

export function meta({}: Route.MetaArgs) {
  return [
    { title: "ChessGG" },
    { name: "description", content: "Welcome to React Router!" },
  ];
}

export default function Home() {
  return <MainLayout>
    <HomePage/>
  </MainLayout>
}