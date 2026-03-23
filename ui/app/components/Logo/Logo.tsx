import { useNavigate } from "react-router"

export default function Logo() {
    const navigate = useNavigate()

    return (
        <div onClick={() => navigate("/")} className="cursor-pointer">
            <svg width="420" height="120" viewBox="0 0 420 120" xmlns="http://www.w3.org/2000/svg" fill="none">
                <defs>
                    <linearGradient id="grad" x1="0" y1="0" x2="1" y2="1">
                        <stop offset="0%" stop-color="#6366F1"/>
                        <stop offset="100%" stop-color="#22C55E"/>
                    </linearGradient>

                    <pattern id="chessPattern" patternUnits="userSpaceOnUse" width="10" height="10">
                        <rect width="10" height="10" fill="#0f172a"/>
                        <rect width="5" height="5" fill="#1e293b"/>
                        <rect x="5" y="5" width="5" height="5" fill="#1e293b"/>
                    </pattern>
                </defs>

                <g transform="translate(10, 10)">
                    <rect x="0" y="0" width="100" height="100" rx="20" fill="url(#chessPattern)" />

                    <g transform="translate(0, 10) scale(2, 2)">
                        <polygon 
                            points="25,6 38,15 35,30 25,36 12,28 10,14"
                            fill="url(#grad)"
                            stroke="url(#grad)"
                            stroke-width="2"
                        />

                        <circle cx="25" cy="6" r="2" fill="url(#grad)" />
                        <circle cx="38" cy="15" r="2" fill="url(#grad)" />
                        <circle cx="35" cy="30" r="2" fill="url(#grad)" />
                        <circle cx="25" cy="36" r="2" fill="url(#grad)" />
                        <circle cx="12" cy="28" r="2" fill="url(#grad)" />
                        <circle cx="10" cy="14" r="2" fill="url(#grad)" />
                    </g>
                </g>

                <text x="130" y="65" font-family="Inter, Arial, sans-serif" font-size="42" font-weight="700" fill="#64748b">
                    chess<tspan fill="url(#grad)">GG</tspan>
                </text>

                <text x="132" y="90" font-family="Inter, Arial, sans-serif" font-size="14" fill="#64748b">
                    player analytics
                </text>
            </svg>
        </div>
    )
}