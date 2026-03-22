import MainLayout from "~/layouts/MainLayout";
import type { Route } from "./+types/analysis";
import AnalysisPage from "~/components/AnalysisPage/AnalysisPage";

export function loader({ params }: Route.LoaderArgs) {
    return { player: params.player }
}

export function meta({ params }: Route.MetaArgs) {
  return [
    { title: params.player }
  ];
}

export default function Component({
    loaderData
}: Route.ComponentProps) {
  return <MainLayout>
    <AnalysisPage />
  </MainLayout>
}