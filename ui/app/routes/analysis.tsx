import type { Route } from "./+types/analysis";

export function loader({ params }: Route.LoaderArgs) {
    return { player: params.player }
}

export default function Component({
    loaderData
}: Route.ComponentProps) {
    return (
        <div>
            Empty Analysis Page ({loaderData.player})
        </div>
    )
}