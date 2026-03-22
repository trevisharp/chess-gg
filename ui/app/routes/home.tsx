import type { Route } from "./+types/home";

export function meta({}: Route.MetaArgs) {
  return [
    { title: "ChessGG" },
    { name: "description", content: "Welcome to React Router!" },
  ];
}

export default function Home() {
  return (<div>
    Empty Home Page
  </div>)
}
