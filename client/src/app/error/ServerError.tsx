import { Container, Divider, Paper, Typography } from "@mui/material";
import { useLocation } from "react-router-dom";

export default function ServerError() {
  const { state } = useLocation();

  return (
    <Container component={Paper}>
      {state?.error ? (
        <>
          <Typography color="secondary" variant="h3" gutterBottom>
            {state.error.title}
          </Typography>
          <Divider />
          <Typography variant="body1">
            {state.error.detail || "Internal Server Error"}
          </Typography>
        </>
      ) : (
        <Typography gutterBottom variant="h5">
          Server Error
        </Typography>
      )}
    </Container>
  );
}
